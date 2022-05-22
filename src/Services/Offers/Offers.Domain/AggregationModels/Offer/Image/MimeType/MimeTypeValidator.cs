using FluentValidation;

namespace Offers.Domain.AggregationModels.Offer.Image.MimeType;

public class MimeTypeValidator: AbstractValidator<MimeTypeAggregate>
{
    public MimeTypeValidator()
    {
        RuleFor(x => x.Type)
            .Matches(@"\w+\/[-+.\w]+")
            .WithMessage("Type name does not match mime type regex. Is it valid?");
    }
}
