using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Offers.Domain.AggregationModels.Offer;
using Offers.Domain.AggregationModels.Offer.Category;
using Offers.Domain.AggregationModels.Offer.Image;
using Offers.Domain.AggregationModels.Offer.Image.MimeType;
using Offers.Domain.AggregationModels.Offer.Location;
using Offers.Infrastructure.Data;
using Offers.Infrastructure.Repositories;

namespace Offers.Api.Configuration;

public static class ServicesConfiguration
{
    private static string connectionString { get; set; }
    private static string migrationsAssembly = "Offers.Infrastructure";
    
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder app)
    {
        connectionString = app.Configuration.GetConnectionString("OffersDb");

        app.ConfigureServicesLifetime()
            .ConfigureDbContext()
            .ConfigureValidators();
        
        return app;
    }
    
    private static WebApplicationBuilder ConfigureValidators(this WebApplicationBuilder app)
    {
        app.Services
            .AddFluentValidation(x => x.RegisterValidatorsFromAssemblies(new[]
            {
                typeof(Program).Assembly,
                typeof(OfferAggregateRoot).Assembly
            }));
        return app;
    }
    
    private static WebApplicationBuilder ConfigureServicesLifetime(this WebApplicationBuilder app)
    {
        app.Services.AddScoped<IMimeTypeRepository, MimeTypeRepository>();
        app.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        app.Services.AddScoped<IImageRepository, ImageRepository>();
        app.Services.AddScoped<ILocationRepository, LocationRepository>();
        app.Services.AddScoped<IOfferRepository, OfferRepository>();
        
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
