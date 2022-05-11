using System.Reflection;
using System.Runtime.CompilerServices;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation.AspNetCore;
using Identity.Api.Configuration.Auth;
using Identity.Application.DTO;
using Identity.Application.DTO.RegisteringUser;
using Identity.Application.Mappers;
using Identity.Application.Mappers.UserMapper;
using Identity.Application.Mappers.UserMapper.AddressMapper;
using Identity.Application.Mappers.UserMapper.CountryInfoMapper;
using Identity.Domain.AggregationModels.ApplicationUser;
using Identity.Domain.AggregationModels.ApplicationUser.Address;
using Identity.Domain.AggregationModels.ApplicationUser.Address.CountryInfo;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBusRabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Subscriptions;

namespace Identity.Api.Configuration;

public static class ServicesConfiguration
{
    private static string connectionString { get; set; }
    private static string migrationsAssembly = "Identity.Infrastructure";
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder app)
    {
        app.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

        connectionString = app.Configuration.GetConnectionString("IdentityDb");
        migrationsAssembly = "Identity.Infrastructure";

        app.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        app.ConfigureServicesLifetime()
            .AddCustomIntegrations()
            .ConfigureDbContext()
            .ConfigureIdentity()
            .ConfigureIdentityServer()
            .AddEventBus()
            .ConfigureSpa();
        return app;
    }
    
    private static WebApplicationBuilder ConfigureServicesLifetime(this WebApplicationBuilder app)
    {
        app.Services.AddScoped<IApplicationUserMapper, ApplicationUserMapper>();
        app.Services.AddScoped<IAddressMapper, AddressMapper>();
        app.Services.AddScoped<ICountryInfoMapper, CountryInfoMapper>();
        
        app.Services.AddScoped<ICountryRepository, CountryRepository>();
        
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
        app.Services.AddIdentity<ApplicationUserAggregateRoot, IdentityRole>()
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
            .AddAspNetIdentity<ApplicationUserAggregateRoot>()
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
                typeof(ApplicationUserAggregateRoot).Assembly
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

    public static WebApplicationBuilder AddEventBus(this WebApplicationBuilder app)
    {
        if (app.Configuration.GetValue<bool>("RabbitMqServiceBusEnabled"))
        {
            app.Services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var subscriptionClientName = app.Configuration["SubscriptionClientName"];
                var rabbitMQConnection = sp.GetRequiredService<IRabbitMQConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionManager>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(app.Configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(app.Configuration["EventBusRetryCount"]);
                }

                return new EventBusRabbitMQ(rabbitMQConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            });
        }

        app.Services.AddSingleton<IEventBusSubscriptionManager, InMemoryEventBusSubscriptionsManager>();

        return app;
    }

    public static WebApplicationBuilder AddCustomIntegrations(this WebApplicationBuilder app)
    {
        app.Services.AddSingleton<IRabbitMQConnection>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<RabbitMQConnection>>();

            var factory = new ConnectionFactory()
            {
                HostName = app.Configuration["EventBusConnection"],
                DispatchConsumersAsync = true
            };

            if (!string.IsNullOrEmpty(app.Configuration["EventBusUserName"]))
            {
                factory.UserName = app.Configuration["EventBusUserName"];
            }

            if (!string.IsNullOrEmpty(app.Configuration["EventBusPassword"]))
            {
                factory.Password = app.Configuration["EventBusPassword"];
            }

            var retryCount = 5;
            if (!string.IsNullOrEmpty(app.Configuration["EventBusRetryCount"]))
            {
                retryCount = int.Parse(app.Configuration["EventBusRetryCount"]);
            }

            return new RabbitMQConnection(factory, logger, retryCount);
        });

        return app;
    }

    public static WebApplication ConfigureEventBus(this WebApplication app)
    {
        var eventBus = app.Services.GetRequiredService<IEventBus>();
        
        return app;
    }
}
