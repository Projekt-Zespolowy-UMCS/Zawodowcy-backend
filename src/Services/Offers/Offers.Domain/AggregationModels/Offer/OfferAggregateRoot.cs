using FluentValidation;
using Offers.Domain.AggregationModels.Offer.Category;
using Offers.Domain.AggregationModels.Offer.Image;
using Offers.Domain.AggregationModels.Offer.Location;
using Offers.Domain.Base;

namespace Offers.Domain.AggregationModels.Offer;

public class OfferAggregateRoot: BaseEntityUnique
{
    public Guid AuthorId { get; protected set; }
    public string Title { get; protected set; }
    public string Content { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public bool IsArchival { get; protected set; }

    public int CategoryId { get; protected set; }
    public CategoryAggregate Category { get; protected set; }
    
    public int LocationId { get; protected set; }
    public LocationAggregate Location { get; protected set; }

    public IList<ImageAggregate> Images { get; protected set; }

    protected OfferAggregateRoot()
    {
    }
    
    public OfferAggregateRoot(Guid authorId, string title, string content, DateTime createdAt, DateTime? updatedAt, 
    bool isArchival, CategoryAggregate category, int locationId, LocationAggregate location, IList<ImageAggregate> images)
    {
        AuthorId = authorId;
        Title = string.IsNullOrWhiteSpace(title) ? throw new ArgumentException(nameof(title)) : title;
        Content = string.IsNullOrWhiteSpace(content) ? throw new ArgumentException(nameof(content)): content;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt?.Date;
        IsArchival = isArchival;
        Category = category ?? throw new ArgumentException(nameof(category));
        CategoryId = category.Id;
        Location = location ?? throw new ArgumentException(nameof(location));
        LocationId = locationId;
        Images = images ?? throw new ArgumentException(nameof(images));
        
        new OfferValidator().ValidateAndThrow(this);
    }
}
