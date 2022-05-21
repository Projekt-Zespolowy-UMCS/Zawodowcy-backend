using Offers.Api.Extensions;
using Offers.Infrastructure.Data;
using Offers.Infrastructure.Data.Seeders;

namespace Offers.Api.Configuration;

public static class DatabaseConfiguration
{
    public static WebApplication ConfigureDatabase(this WebApplication app)
    {
        ApplyMigrations(app);
        return app;
    }
    
    private static void ApplyMigrations(WebApplication app)
    {
        app.MigrateDbContext<OffersDbContext>((_, __) => { })
            .MigrateDbContext<OffersDbContext>((context, services) =>
            {
                new OffersDbContextSeed()
                    .SeedAsync(services)
                    .Wait();
            });
    }
}
