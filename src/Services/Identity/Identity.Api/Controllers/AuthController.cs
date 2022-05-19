using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Identity;
using Duende.IdentityServer.Extensions;
using Identity.Api.DTO;
using Identity.Application.DTO.RegisteringUser;
using Identity.Application.Mappers;
using Identity.Application.Mappers.UserMapper;
using Identity.Application.Mappers.UserMapper.AddressMapper;
using Identity.Domain.AggregationModels.ApplicationUser;
using Identity.Domain.AggregationModels.ApplicationUser.Address;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace idsserver
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;
        private readonly SignInManager<ApplicationUserAggregateRoot> _manager;
        private readonly UserManager<ApplicationUserAggregateRoot> _usermanager;
        private readonly IAddressRepository _addressRepository;
        private readonly IPersistedGrantService _grantService;
        private readonly IPersistedGrantStore _grantStore;
        private readonly ILogger<AuthController> _logger;
        private readonly IApplicationUserMapper _userMapper;

        public AuthController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            SignInManager<ApplicationUserAggregateRoot> manager,
            UserManager<ApplicationUserAggregateRoot> usermanager,
            IAddressRepository addressRepository,
            IPersistedGrantService grantService,
            IPersistedGrantStore grantStore,
            ILogger<AuthController> logger,
            IApplicationUserMapper userMapper)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _manager = manager;
            _usermanager = usermanager;
            _addressRepository = addressRepository;
            _grantService = grantService;
            _grantStore = grantStore;
            _logger = logger;
            _userMapper = userMapper;
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            if (string.IsNullOrEmpty(model?.Username) || string.IsNullOrEmpty(model?.Password))
                return BadRequest("invalid request payload");

            var users = await _manager.UserManager.Users.ToListAsync();
            var user = await _manager.UserManager.FindByEmailAsync(model.Username);

            if (user != null)
            {
                var result = await _manager.PasswordSignInAsync(user, model.Password, model.RememberLogin, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));

                    if (context != null)
                    {
                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return Ok(new
                        {
                            ReturnUrl = model.ReturnUrl
                        });
                    }

                    // request for a local page
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Ok(new
                        {
                            ReturnUrl = model.ReturnUrl
                        });
                    }
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Ok(new
                        {
                            // when user navigate directly to Identity Server, we just take them to home after login
                            ReturnUrl = "/home"
                        });
                    }
                    else
                    {
                        // user might have clicked on a malicious link - should be logged
                        return BadRequest("invalid return URL");
                    }
                }
                if (result.RequiresTwoFactor)
                {
                    return Unauthorized(new
                    {
                        Require2fa = true,
                        ReturnUrl = model.ReturnUrl,
                        RememberMe = model.RememberLogin
                    });
                }
                if (result.IsLockedOut)
                {

                    return Unauthorized(new
                    {
                        Lockeout = true,
                        ReturnUrl = model.ReturnUrl,
                        RememberMe = model.RememberLogin
                    });
                }
                else
                {
                    return BadRequest("Invalid login attempt.");
                }
            }

            await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId: context?.Client.ClientId));
            return BadRequest("Something went wrong");
        }

        /// <summary>
        /// Register new user
        /// </summary>
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterApplicationUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var newUser = _userMapper.MapToEntity(dto);

                if (newUser.Address is not null)
                {
                    var addedAddress = await _addressRepository.CreateAddressAsync(newUser.Address);
                    newUser.SetUserAddress(addedAddress);
                }

                var result = await _usermanager.CreateAsync(newUser, dto.Password);
                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        /// <summary>
        /// End all refresh tokens for the logged in user, accept the current on
        /// </summary>
        /// <returns></returns>
        [Route("endOtherSessions")]
        [HttpGet]
        public async Task<IActionResult> EndAllOtherSessions()
        {
            if (User?.Identity.IsAuthenticated == true)
            {
                // get user id (which is the SubjectID)
                var subjectId = User.Identity.GetSubjectId();
                var user = await this._usermanager.FindByIdAsync(subjectId);
                this._logger.LogInformation($"end all other session for user {user.Email} with subject Id {subjectId}");

                // get the current SessionID for this user
                var result = await HttpContext.AuthenticateAsync();
                var sid = result.Properties.Items.FirstOrDefault(x => x.Key == "session_id").Value;
                this._logger.LogInformation($"current Session ID is {sid}");

                // get all for this user
                var allSessions = await this._grantStore.GetAllAsync(new PersistedGrantFilter
                {
                    SubjectId = subjectId
                });

                this._logger.LogInformation($"this user has {allSessions.Count()} sessions");
                foreach (var s in allSessions)
                {
                    var data = s.Data;
                    if (s.SessionId != sid)
                    {
                        // remove the session 
                        // when we hook this into DB, it will result in 1 call to the DB
                        await this._grantService.RemoveAllGrantsAsync(subjectId, s.ClientId, s.SessionId);
                        this._logger.LogInformation($"killed session Id {s.SessionId}, client ID: {s.ClientId}");
                    }
                }

                //  this will make sure that all other sessions are killed

                await this._usermanager.UpdateSecurityStampAsync(user);
                this._logger.LogInformation($"update Security Stampt");
            }
            return Ok();
        }


        /// <summary>
        /// End all refresh tokens for the logged in user
        /// </summary>
        /// <returns></returns>
        [Route("endAllSessions")]
        [HttpGet]
        public async Task<IActionResult> EndAllSessions()
        {
            if (User?.Identity.IsAuthenticated == true)
            {
                // get user id (which is the SubjectID)
                var subjectId = User.Identity.GetSubjectId();
                var user = await this._usermanager.FindByIdAsync(subjectId);
                this._logger.LogInformation($"end all other session for user {user.Email} with subject Id {subjectId}");
                await this._grantService.RemoveAllGrantsAsync(subjectId);

                //  this will make sure that all other sessions are killed
                await this._usermanager.UpdateSecurityStampAsync(user);
            }
            return Ok();
        }

        [Route("validateSecurityStamp")]
        [HttpGet]
        public async Task<IActionResult> ValidateSecurityStamp()
        {
            if (User?.Identity.IsAuthenticated == true)
            {
                // get user id (which is the SubjectID)
                var validationResponse = await this._manager.ValidateSecurityStampAsync(User);
                return Ok(validationResponse == null ? "empty" : "not_empty");
            }
            return Ok("nothing");
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout(string logoutId)
        {

           /* await _manager.SignOutAsync();

            var logoutRequest = _interaction.GetLogoutContextAsync(logoutId);

            if (string.IsNullOrEmpty(logoutRequest.PostLogoutRedirectUri))
            {
                //return RedirectToAction("/", "")
            }
            
            */

            
            var context = await _interaction.GetLogoutContextAsync(logoutId);
            bool showSignoutPrompt = true;

            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                showSignoutPrompt = true;
            }

            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await _manager.SignOutAsync();
                
            }

            // no external signout supported for now (see \Quickstart\Account\AccountController.cs TriggerExternalSignout)
            return Ok(new
            {
                showSignoutPrompt,
                ClientName = string.IsNullOrEmpty(context?.ClientName) ? context?.ClientId : context?.ClientName,
                context?.PostLogoutRedirectUri,
                context?.SignOutIFrameUrl,
                logoutId
            });
            
        }

    }
}
