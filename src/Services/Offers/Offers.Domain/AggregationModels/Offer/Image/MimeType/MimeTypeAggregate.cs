using FluentValidation;
using Offers.Domain.Base;

namespace Offers.Domain.AggregationModels.Offer.Image.MimeType;

public sealed class MimeTypeAggregate: BaseEntity
{
    public string Type { get; private set; }

    public MimeTypeAggregate(string type)
    {
        Type = string.IsNullOrWhiteSpace(type) ? throw new ArgumentException(nameof(Type)) : type;

        new MimeTypeValidator().ValidateAndThrow(this);
    }
}
