using FluentValidation;
using Identity.Domain.Base;

namespace Identity.Domain.AggregationModels.ApplicationUser.ValueObjects;

public class CountryInfo: BaseEntity
{
    public string ISO { get; private set; }
    public string Name { get; private set; }

    /// <summary>
    /// For Entity Framework purposes
    /// </summary>
    private CountryInfo() {}
    
    public CountryInfo(string country, string region)
    {
        ISO = country ?? throw new ArgumentException(nameof(country));
        Name = region ?? throw new ArgumentException(nameof(region));

        new CountryInfoValidator().ValidateAndThrow(this);
    }
}
