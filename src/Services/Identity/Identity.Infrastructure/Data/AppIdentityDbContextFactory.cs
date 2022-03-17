using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Identity.Infrastructure.Data;

public class AppIdentityDbContextFactory: IDesignTimeDbContextFactory<AppIdentityDbContext>
{
    public readonly IConfiguration _configuration;
    
    public AppIdentityDbContextFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public AppIdentityDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppIdentityDbContext>()
            .UseNpgsql(_configuration.GetConnectionString("IdentityDb"));
        return new AppIdentityDbContext(optionsBuilder.Options);
    }
}