using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel.Client;

namespace Identity.Api.Configuration.Auth;

public class IdentityAuthConfig
{
    public static IEnumerable<ApiResource> GetApis()
        {            
            return new List<ApiResource>
            {
            };
        }

        // Identity resources are data like user ID, name, or email address of a user
        // see: http://docs.identityserver.io/en/release/configuration/resources.html
        public static IEnumerable<IdentityResource> GetResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        // client want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients(Dictionary<string, string> clientsUrl)
        {
            return new List<Client>
            {
                // example
                new Client
                {
                    ClientId = "spa",
                    ClientName = "Zawodowcy SPA OpenId Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,
                    ClientSecrets = { new Secret("secret".Sha256())},
                    RedirectUris =           { $"{clientsUrl["SPA_REDIRECT_URI"]}" },
                    RequireClientSecret = false,
                    RequirePkce = true,
                    PostLogoutRedirectUris = { $"{clientsUrl["SPA_POST_LOGOUT_REDIRECT_URI"]}" },
                    AllowedCorsOrigins = { $"{clientsUrl["SPA_URI"]}" },
                    AllowOfflineAccess = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    },
                }
            };
        }
}
