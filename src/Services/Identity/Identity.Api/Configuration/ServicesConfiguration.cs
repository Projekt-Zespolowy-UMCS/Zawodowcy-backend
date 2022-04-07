using System.Reflection;
using System.Runtime.CompilerServices;
using AutoMapper;
using FluentValidation.AspNetCore;
using Identity.Api.Configuration.Auth;
using Identity.Application.DTO;
using Identity.Application.DTO.RegisteringUser;
using Identity.Application.Mappers;
using Identity.Application.Mappers.UserMapper;
using Identity.Domain.AggregationModels.ApplicationUser;
using Identity.Domain.AggregationModels.ApplicationUser.ValueObjects;
using Identity.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Api.Configuration;

public static class ServicesConfiguration
{
    private static string connectionString { get; set; }
    private static string migrationsAssembly = "Identity.Infrastructure";
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder app)
    {
        connectionString = app.Configuration.GetConnectionString("IdentityDb");
        migrationsAssembly = "Identity.Infrastructure";

        app.ConfigureServicesLifetime()
            .ConfigureDbContext()
            .ConfigureIdentity()
            .ConfigureIdentityServer()
            .ConfigureSpa();
        return app;
    }
    
    
    private static WebApplicationBuilder ConfigureServicesLifetime(this WebApplicationBuilder app)
    {
        app.Services.AddScoped<IMapper<CountryInfo, CountryInfoDto>, CountryInfoMapper>();
        app.Services.AddScoped<IMapper<ApplicationUser, RegisterApplicationUserDto>, ApplicationUserMapper>();
        
        return app;
    }

    private static WebApplicationBuilder ConfigureDbContext(this WebApplicationBuilder app)
    {
        app.Services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseNpgsql(connectionString,
                npgsqlOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(migrationsAssembly);
                    //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
                }));
        return app;
    }

    private static WebApplicationBuilder ConfigureIdentity(this WebApplicationBuilder app)
    {
        app.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();
        return app;
    }
    
    private static WebApplicationBuilder ConfigureIdentityServer(this WebApplicationBuilder app)
    {
        app.Services.AddIdentityServer(x =>
            {
                x.IssuerUri = "null";
                x.Authentication.CookieLifetime = TimeSpan.FromHours(2);
                x.UserInteraction.LoginUrl = "/login";
                x.UserInteraction.LogoutUrl = "/logout/";
            })
            .AddDeveloperSigningCredential()
            .AddAspNetIdentity<ApplicationUser>()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder => builder.UseNpgsql(connectionString,
                    npgsqlOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(migrationsAssembly);
                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorCodesToAdd: null);
                    });
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder => builder.UseNpgsql(connectionString,
                    npgsqlOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(migrationsAssembly);
                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorCodesToAdd: null);
                    });
            });
        return app;
    }

    private static WebApplicationBuilder ConfigureValidators(this WebApplicationBuilder app)
    {
        app.Services
            .AddFluentValidation(x => x.RegisterValidatorsFromAssemblies(new[]
            {
                typeof(Program).Assembly,
                typeof(ApplicationUser).Assembly
            }));
        return app;
    }

    private static WebApplicationBuilder ConfigureSpa(this WebApplicationBuilder app)
    {
        app.Services.AddSpaStaticFiles(config =>
        {
            if (app.Environment.IsDevelopment())
            {
                config.RootPath = "client-ui";
            }
            else
            {
                config.RootPath = "client-ui/build";
            }
        });
        return app;
    }
}
