using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Identity.Infrastructure.Data;

public class ConfigurationDbContextFactory: IDesignTimeDbContextFactory<ConfigurationDbContext>
{
    public ConfigurationDbContext CreateDbContext(string[] args)
    {
        var configPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
        
        var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(configPath))
            .AddJsonFile("Identity.Api/appsettings.Development.json")
            .AddEnvironmentVariables()
            .Build();
        
        var optionsBuilder = new DbContextOptionsBuilder<ConfigurationDbContext>();
        var storeOptions = new ConfigurationStoreOptions();
        
        optionsBuilder.UseNpgsql(config.GetConnectionString("IdentityDb"), options => options.MigrationsAssembly("Identity.Infrastructure"));
        return new ConfigurationDbContext(optionsBuilder.Options);
    }
}