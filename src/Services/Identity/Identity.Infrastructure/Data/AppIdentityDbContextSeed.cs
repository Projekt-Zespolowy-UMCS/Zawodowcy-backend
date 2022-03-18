using System.Globalization;
using Identity.Domain.AggregationModels.ApplicationUser;
using Identity.Domain.AggregationModels.ApplicationUser.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.Data;

public class AppIdentityDbContextSeed
{
    public readonly PasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();

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
                "admin",
                "TEST@TEST.COM",
                "");

        user.PasswordHash = _passwordHasher.HashPassword(user, "Pass@word1");

        return new List<ApplicationUser>()
        {
            user
        };
    }
}