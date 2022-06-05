using FluentValidation;
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
    public MimeTypeAggregate MimeType { get; protected set; }
    public Guid OfferId { get; protected set; }
    public OfferAggregateRoot Offer { get; protected set; }

    public ImageAggregate(string name, string absolutePath, DateTime createdAt, DateTime? updatedAt, int mimeTypeId, MimeTypeAggregate mimeType, Guid offerId, OfferAggregateRoot offer)
    {
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException(nameof(name)) : name;
        AbsolutePath = string.IsNullOrWhiteSpace(absolutePath) ? throw new ArgumentException(nameof(absolutePath)) : absolutePath;
        CreatedAt = createdAt == DateTime.MinValue ? throw new ArgumentException(nameof(createdAt)) : createdAt;
        UpdatedAt = updatedAt == DateTime.MinValue ? throw new ArgumentException(nameof(updatedAt)) : updatedAt;
        MimeTypeId = mimeTypeId == 0 ? throw new ArgumentException(nameof(mimeTypeId)) : mimeTypeId;
        MimeType = mimeType ?? throw new ArgumentException(nameof(mimeType));
        OfferId = offerId;
        Offer = offer ?? throw new ArgumentException(nameof(offer));

        new ImageValidator().ValidateAndThrow(this);
    }
}
