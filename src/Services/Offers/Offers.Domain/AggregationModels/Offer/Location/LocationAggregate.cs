using Offers.Domain.Base;

namespace Offers.Domain.AggregationModels.Offer.Location;

public sealed class LocationAggregate: BaseEntity
{
    public string Name { get; protected set; }
    public string CountryISO { get; protected set; }

    protected LocationAggregate() {}

    public LocationAggregate(string name, string countryIso)
    {
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException(nameof(name)) : name;
        CountryISO = string.IsNullOrWhiteSpace(countryIso) ? throw new ArgumentException(nameof(countryIso)) : countryIso;;
    }
}
