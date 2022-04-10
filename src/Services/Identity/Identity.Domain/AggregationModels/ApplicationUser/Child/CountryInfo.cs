using FluentValidation;
using Identity.Domain.Base;

namespace Identity.Domain.AggregationModels.ApplicationUser.Child;

public class CountryInfo: BaseEntity
{
    public string ISO { get; private set; }
    public string Name { get; private set; }

    /// <summary>
    /// For Entity Framework purposes
    /// </summary>
    private CountryInfo() {}
    
    public CountryInfo(string iso, string name)
    {
        ISO = iso ?? throw new ArgumentException(nameof(iso));
        Name = name ?? throw new ArgumentException(nameof(name));

        new CountryInfoValidator().ValidateAndThrow(this);
    }
}
