using FluentValidation;

namespace Offers.Domain.AggregationModels.Offer.Image;

public class ImageValidator: AbstractValidator<ImageAggregate>
{
    public ImageValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(ImageValidationConstants.NameMinimumLength)
            .WithMessage($"Name is too short. It has to be minimum {ImageValidationConstants.NameMinimumLength} chars length.")
            .MaximumLength(ImageValidationConstants.NameMaxinumLength)
            .WithMessage($"Name is too short. It has to be maximum {ImageValidationConstants.NameMaxinumLength} chars length.");

        RuleFor(x => x.AbsolutePath)
            .NotEmpty()
            .WithMessage("Image path cannot be empty.");

        RuleFor(x => x.CreatedAt)
            .NotNull()
            .WithMessage("Created date cannot be null.");

        RuleFor(x => x.MimeType)
            .NotEmpty()
            .WithMessage("Message mime type is required.");

        When(x => x.UpdatedAt.HasValue, () =>
        {
            RuleFor(x => x.UpdatedAt.Value)
                .GreaterThan(DateTime.MinValue)
                .WithMessage("Update time cannot be default one.");
        });
    }
}
