using Duende.IdentityServer.EntityFramework.DbContexts;
using Identity.Api.Extensions;
using Identity.Api.Utils;
using Identity.Domain.AggregationModels.ApplicationUser;
using Identity.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Configuration;

public static class DatabaseConfiguration
{
    public static WebApplication ConfigureDatabase(this WebApplication app)
    {
        ApplyMigrations(app);
        return app;
    }
    
    private static void ApplyMigrations(WebApplication app)
    {
        app.MigrateDbContext<PersistedGrantDbContext>((_, __) => { })
            .MigrateDbContext<AppIdentityDbContext>((context, services) =>
            {
                new AppIdentityDbContextSeed()
                    .SeedAsync(services)
                    .Wait();
            })
            .MigrateDbContext<ConfigurationDbContext>((context, services) =>
            {
                new ConfigurationDbContextSeed()
                    .SeedAsync(context, StartupUtils.GetConfiguration())
                    .Wait();
            });
    }
}
