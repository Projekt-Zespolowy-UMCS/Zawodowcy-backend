using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Identity.Infrastructure.Data;

public class AppIdentityDbContextFactory: IDesignTimeDbContextFactory<AppIdentityDbContext>
{
    public AppIdentityDbContext CreateDbContext(string[] args)
    {
        var configPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
        
        var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(configPath))
            .AddJsonFile("Identity.Api/appsettings.Development.json")
            .AddEnvironmentVariables()
            .Build();
        
        var optionsBuilder = new DbContextOptionsBuilder<AppIdentityDbContext>()
            .UseNpgsql(config.GetConnectionString("IdentityDb"), options => options.MigrationsAssembly("Identity.Infrastructure"));
        return new AppIdentityDbContext(optionsBuilder.Options);
    }
}