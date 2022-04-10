using System.Globalization;
using System.Security.Cryptography.Xml;
using Identity.Domain.AggregationModels.ApplicationUser;
using Identity.Domain.AggregationModels.ApplicationUser.Child;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Identity.Infrastructure.Data;

public class AppIdentityDbContextSeed
{
    public readonly PasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();
    public async Task SeedAsync(IServiceProvider serviceProvider, int? retry = 0)
    {
        int retryCounter = retry.Value;

        try
        {
            SeedUsers(serviceProvider);
            SeedCountries(serviceProvider);
        }
        catch (Exception ex)
        {
            if (retryCounter < 10)
            {
                retryCounter++;
                await SeedAsync(serviceProvider, retryCounter);
            }
        }
    }

    private async Task SeedUsers(IServiceProvider serviceProvider)
    {
        using var scopes = serviceProvider.CreateScope();
        var userManager = scopes.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        if (!userManager.Users.Any())
        {
            await userManager.CreateAsync(GetDefaultUser(), "Pass@word1");
        }
    }
    
    private async Task SeedCountries(IServiceProvider serviceProvider)
    {
        using var scopes = serviceProvider.CreateScope();
        var context = scopes.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
        if (!context.CountryInfos.Any())
        {
            await context.CountryInfos.AddRangeAsync(GetCountriesList());
            await context.SaveChangesAsync();
        }
    }

    private ApplicationUser GetDefaultUser()
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

        return user;
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
