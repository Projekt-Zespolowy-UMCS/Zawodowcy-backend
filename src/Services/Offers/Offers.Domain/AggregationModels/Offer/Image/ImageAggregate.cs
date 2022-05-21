using Offers.Domain.AggregationModels.Offer.Image.MimeType;
using Offers.Domain.Base;

namespace Offers.Domain.AggregationModels.Offer.Image;

public class ImageAggregate: BaseEntityUnique
{
    public string Name { get; protected set; }
    public string AbsolutePath { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }

    public int MimeTypeId { get; protected set; }
    public MimeTypeAggregate MimeType { get; set; }

    protected ImageAggregate() {}
}
