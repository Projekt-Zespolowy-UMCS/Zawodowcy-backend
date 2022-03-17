using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Configuration;

public static class ServicesConfiguration
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder app)
    {
        var connectionString = app.Configuration.GetConnectionString("IdentityDb");
        app.Services.AddPooledDbContextFactory<AppIdentityDbContext>(options =>
            options.UseNpgsql(connectionString));

        return app;
    }
}