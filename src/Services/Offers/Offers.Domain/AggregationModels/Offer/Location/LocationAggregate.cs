using Offers.Domain.Base;

namespace Offers.Domain.AggregationModels.Offer.Location;

public sealed class LocationAggregate: BaseEntity
{
    public string Name { get; protected set; }
    public string CountryISO { get; protected set; }

    protected LocationAggregate() {}
}
