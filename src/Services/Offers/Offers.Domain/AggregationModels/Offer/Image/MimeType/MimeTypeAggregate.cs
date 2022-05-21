using Offers.Domain.Base;

namespace Offers.Domain.AggregationModels.Offer.Image.MimeType;

public sealed class MimeTypeAggregate: BaseEntity
{
    public string Type { get; private set; }
}
