using Microsoft.EntityFrameworkCore;
using Offers.Infrastructure.Data;

namespace Offers.Api.Configuration;

public static class ServicesConfiguration
{
    private static string connectionString { get; set; }
    private static string migrationsAssembly = "Offers.Infrastructure";
    
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder app)
    {
        connectionString = app.Configuration.GetConnectionString("OffersDb");

        app.ConfigureServicesLifetime()
            .ConfigureDbContext();
        
        return app;
    }
    
    private static WebApplicationBuilder ConfigureServicesLifetime(this WebApplicationBuilder app)
    {
        return app;
    }

    private static WebApplicationBuilder ConfigureDbContext(this WebApplicationBuilder app)
    {
        app.Services.AddDbContext<OffersDbContext>(options =>
            options.UseNpgsql(connectionString,
                npgsqlOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(migrationsAssembly);
                    //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
                }));
        return app;
    }

}
