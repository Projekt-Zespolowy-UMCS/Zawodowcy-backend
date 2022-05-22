using FluentValidation;

namespace Offers.Domain.AggregationModels.Offer;

public class OfferValidator: AbstractValidator<OfferAggregateRoot>
{
    public OfferValidator()
    {
        RuleFor(x => x.Content)
            .MinimumLength(OfferValidationConstants.ContentMinimumLength)
            .WithMessage($"Content is too short. It has to be minimum {OfferValidationConstants.ContentMinimumLength} chars length.")
            .MaximumLength(OfferValidationConstants.ContentMaximumLength)
            .WithMessage($"Content is too short. It has to be maximum {OfferValidationConstants.ContentMaximumLength} chars length.");
        
        RuleFor(x => x.Category)
            .NotNull()
            .WithMessage("Category is required.");

        RuleFor(x => x.Location)
            .NotNull()
            .WithMessage("Location is requierd.");
        
        RuleFor(x => x.CreatedAt)
            .GreaterThan(DateTime.MinValue)
            .WithMessage("Created date cannot be default value.");

        When(x => x.UpdatedAt.HasValue, () =>
        {
            RuleFor(x => x.UpdatedAt)
                .GreaterThan(DateTime.MinValue)
                .WithMessage("Update date cannot be default value.");
        });
    }
}
