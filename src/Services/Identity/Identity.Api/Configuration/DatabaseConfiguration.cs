using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Configuration;

public static class DatabaseConfiguration
{
    public static WebApplication ConfigureDatabase(this WebApplication app)
    {
        ApplyMigrations(app);
        return app;
    }
    
    private static void ApplyMigrations(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
    
            var context = services.GetRequiredService<IDbContextFactory<AppIdentityDbContext>>().CreateDbContext();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
    }
}