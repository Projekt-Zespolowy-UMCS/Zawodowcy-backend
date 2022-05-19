using System.Globalization;
using System.Security.Cryptography.Xml;
using Identity.Domain.AggregationModels.ApplicationUser;
using Identity.Domain.AggregationModels.ApplicationUser.Address;
using Identity.Domain.AggregationModels.ApplicationUser.Address.CountryInfo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Identity.Infrastructure.Data;

public class AppIdentityDbContextSeed
{
    public readonly PasswordHasher<ApplicationUserAggregateRoot> _passwordHasher = new();
    public async Task SeedAsync(IServiceProvider serviceProvider, int? retry = 0)
    {
        int retryCounter = retry.Value;

        try
        {
            SeedCountries(serviceProvider);
            SeedUsers(serviceProvider);
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
        var userManager = scopes.ServiceProvider.GetRequiredService<UserManager<ApplicationUserAggregateRoot>>();
        var context = scopes.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
        if (!userManager.Users.Any())
        {
            var defaultUser = GetDefaultUser();
            await context.Addresses.AddAsync(defaultUser.Address);
            await userManager.CreateAsync(GetDefaultUser(), "Pass@word1");
        }
        
        await context.SaveChangesAsync();
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

    private ApplicationUserAggregateRoot GetDefaultUser()
    {
        var user =
            new ApplicationUserAggregateRoot(
                "Andrzej",
                "Daniluk",
                "test@test.com",
                "123456789",
                GetDefaultAddress());

        user.PasswordHash = _passwordHasher.HashPassword(user, "Pass@word1");

        return user;
    }

    private AddressAggregate GetDefaultAddress()
    {
        var regionInfo = new RegionInfo("PL");
        
        return new AddressAggregate(
            "Wiejska",
            "Warszawa",
            "Mazowieckie",
            new CountryInfoAggregate(regionInfo.ThreeLetterISORegionName, regionInfo.EnglishName),
            "08500");
    }

    private IEnumerable<CountryInfoAggregate> GetCountriesList()
    {
        var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
        IEnumerable<CountryInfoAggregate> countries = new List<CountryInfoAggregate>();
        foreach (var culture in cultures)
        {
            var region = new RegionInfo(culture.Name);
            if (IsCountryValid(countries, region.ThreeLetterISORegionName, region.EnglishName))
                countries = countries.Append(new CountryInfoAggregate(region.ThreeLetterISORegionName, region.EnglishName));
        }

        countries = countries.OrderBy(x => x.Name);
        return countries;
    }

    private bool IsCountryValid(IEnumerable<CountryInfoAggregate> countries, string iso, string name)
    {
        return iso != String.Empty && 
               name != String.Empty && 
               countries.All(c => c.Name != name && c.ISO != iso);
    }
}
