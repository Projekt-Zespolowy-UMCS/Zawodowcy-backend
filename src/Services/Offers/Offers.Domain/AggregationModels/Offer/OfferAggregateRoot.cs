using Offers.Domain.AggregationModels.Offer.Category;
using Offers.Domain.AggregationModels.Offer.Image;
using Offers.Domain.AggregationModels.Offer.Location;
using Offers.Domain.Base;

namespace Offers.Domain.AggregationModels.Offer;

public class OfferAggregateRoot: BaseEntityUnique
{
    public string AuthorId { get; protected set; }
    public string Title { get; protected set; }
    public string Content { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public bool IsArchival { get; protected set; }

    public int CategoryId { get; protected set; }
    public virtual CategoryAggregate Category { get; protected set; }
    
    public int LocationId { get; protected set; }
    public virtual LocationAggregate Location { get; protected set; }

    public IList<ImageAggregate> Images { get; protected set; }

    protected OfferAggregateRoot() {}
}
