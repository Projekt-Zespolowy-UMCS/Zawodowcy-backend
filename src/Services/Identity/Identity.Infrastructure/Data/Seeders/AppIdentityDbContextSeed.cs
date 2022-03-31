using System.Globalization;
using System.Security.Cryptography.Xml;
using Identity.Domain.AggregationModels.ApplicationUser;
using Identity.Domain.AggregationModels.ApplicationUser.ValueObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
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
            SeedUsers(context);
            SeedCountries(context);
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

    private async Task SeedUsers(AppIdentityDbContext context)
    {
        if (!context.Users.Any())
        {
            await context.Users.AddRangeAsync(GetDefaultUser());
            await context.SaveChangesAsync();
        }
    }
    
    private async Task SeedCountries(AppIdentityDbContext context)
    {
        if (!context.CountryInfos.Any())
        {
            await context.CountryInfos.AddRangeAsync(GetCountriesList());
            await context.SaveChangesAsync();
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
                "test@test.com");

        user.PasswordHash = _passwordHasher.HashPassword(user, "Pass@word1");

        return new List<ApplicationUser>()
        {
            user
        };
    }

    private IEnumerable<CountryInfo> GetCountriesList()
    {
        var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
        IEnumerable<CountryInfo> countries = new List<CountryInfo>();
        foreach (var culture in cultures)
        {
            var region = new RegionInfo(culture.Name);
            if (IsCountryValid(countries, region.ThreeLetterISORegionName, region.EnglishName))
                countries = countries.Append(new CountryInfo(region.ThreeLetterISORegionName, region.EnglishName));
        }

        countries = countries.OrderBy(x => x.Name);
        return countries;
    }

    private bool IsCountryValid(IEnumerable<CountryInfo> countries, string iso, string name)
    {
        return iso != String.Empty && 
               name != String.Empty && 
               countries.All(c => c.Name != name && c.ISO != iso);
    }
}
