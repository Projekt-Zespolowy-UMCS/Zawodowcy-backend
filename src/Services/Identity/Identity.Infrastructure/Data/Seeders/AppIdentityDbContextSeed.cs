using System.Globalization;
using Identity.Domain.AggregationModels.ApplicationUser;
using Identity.Domain.AggregationModels.ApplicationUser.ValueObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Identity.Infrastructure.Data;

public class AppIdentityDbContextSeed
{
    public readonly PasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();

    public async Task SeedAsync(AppIdentityDbContext context,
        ILogger<AppIdentityDbContextSeed> logger, int? retry = 0)
    {
        int retryCounter = retry.Value;

        try
        {
            if (!context.Users.Any())
            {
                await context.Users.AddRangeAsync(GetDefaultUser());
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            if (retryCounter < 10)
            {
                retryCounter++;
                logger.LogError(ex, $"Unable to seed {nameof(AppIdentityDbContext)}");
                await SeedAsync(context, logger, retryCounter);
            }
        }
    }

    private IEnumerable<ApplicationUser> GetDefaultUser()
    {
        var regionInfo = new RegionInfo("PL");

        var user =
            new ApplicationUser(
                "Wiejska",
                "Warszawa",
                "Mazowieckie",
                new CountryInfo(regionInfo.ThreeLetterISORegionName, regionInfo.EnglishName),
                "08500",
                "Andrzej",
                "Daniluk",
                "test@test.com",
                "123456789",
                "test@test.com",
                "TEST@TEST.COM",
                "TEST@TEST.COM");

        user.PasswordHash = _passwordHasher.HashPassword(user, "Pass@word1");

        return new List<ApplicationUser>()
        {
            user
        };
    }
}