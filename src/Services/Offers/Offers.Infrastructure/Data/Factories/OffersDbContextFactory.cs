using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Offers.Infrastructure.Data;

namespace Offers.Infrastructure.Data;

public class OffersDbContextFactory: IDesignTimeDbContextFactory<OffersDbContext>
{
    public OffersDbContext CreateDbContext(string[] args)
    {
        var configPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
        
        var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(configPath))
            .AddJsonFile("Offers.Api/appsettings.Development.json")
            .AddEnvironmentVariables()
            .Build();
        
        var optionsBuilder = new DbContextOptionsBuilder<OffersDbContext>()
            .UseNpgsql(config.GetConnectionString("OffersDb"), options => options.MigrationsAssembly("Offers.Infrastructure"));
        return new OffersDbContext(optionsBuilder.Options);
    }
}
