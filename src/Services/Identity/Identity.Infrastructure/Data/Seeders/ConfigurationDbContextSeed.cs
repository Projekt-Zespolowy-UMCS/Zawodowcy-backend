using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Mappers;
using Identity.Api.Configuration.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Identity.Infrastructure.Data;

public class ConfigurationDbContextSeed
{
    public async Task SeedAsync(ConfigurationDbContext context, IConfiguration configuration)
        {
            var clientUrls = configuration.GetSection("Clients").GetChildren()
                .ToDictionary(x => x.Key, x => x.Value);

            if (!context.Clients.Any())
            {
                foreach (var client in IdentityAuthConfig.GetClients(clientUrls))
                {
                    context.Clients.Add(client.ToEntity());
                }
                await context.SaveChangesAsync();
            }
            // Checking always for old redirects to fix existing deployments
            // to use new swagger-ui redirect uri as of v3.0.0
            // There should be no problem for new ones
            // ref: https://github.com/dotnet-architecture/eShopOnContainers/issues/586
            else
            {
                List<ClientRedirectUri> oldRedirects = (await context.Clients.Include(c => c.RedirectUris).ToListAsync())
                    .SelectMany(c => c.RedirectUris)
                    .Where(ru => ru.RedirectUri.EndsWith("/o2c.html"))
                    .ToList();

                if (oldRedirects.Any())
                {
                    foreach (var ru in oldRedirects)
                    {
                        ru.RedirectUri = ru.RedirectUri.Replace("/o2c.html", "/oauth2-redirect.html");
                        context.Update(ru.Client);
                    }
                    await context.SaveChangesAsync();
                }
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in IdentityAuthConfig.GetResources())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                await context.SaveChangesAsync();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var api in IdentityAuthConfig.GetApis())
                {
                    context.ApiResources.Add(api.ToEntity());
                }

                await context.SaveChangesAsync();
            }
        }
}
