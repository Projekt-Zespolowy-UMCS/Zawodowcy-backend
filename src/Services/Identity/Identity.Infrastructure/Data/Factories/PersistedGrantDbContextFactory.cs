using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Identity.Infrastructure.Data;

public class PersistedGrantDbContextFactory: IDesignTimeDbContextFactory<PersistedGrantDbContext>
{
    public PersistedGrantDbContext CreateDbContext(string[] args)
    {
        var configPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
        
        var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(configPath))
            .AddJsonFile("Identity.Api/appsettings.Development.json")
            .AddEnvironmentVariables()
            .Build();
        
        var optionsBuilder = new DbContextOptionsBuilder<PersistedGrantDbContext>();
        var storeOptions = new OperationalStoreOptions();
        
        optionsBuilder.UseNpgsql(config.GetConnectionString("IdentityDb"), options => options.MigrationsAssembly("Identity.Infrastructure"));
        return new PersistedGrantDbContext(optionsBuilder.Options);
    }
}