using Identity.Api.Extensions;
using Identity.Api.Utils;
using Identity.Infrastructure.Data;
using IdentityServer4.EntityFramework.DbContexts;
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
                var logger = services.GetService<ILogger<AppIdentityDbContextSeed>>();

                new AppIdentityDbContextSeed()
                    .SeedAsync(context, logger)
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