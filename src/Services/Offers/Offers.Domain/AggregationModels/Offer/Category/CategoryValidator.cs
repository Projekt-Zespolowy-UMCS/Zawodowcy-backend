using FluentValidation;

namespace Offers.Domain.AggregationModels.Offer.Category;

public class CategoryValidator: AbstractValidator<CategoryAggregate>
{
    public CategoryValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(CategoryValidationConstants.NameMinimumLength)
            .WithMessage($"Name is too short. It has to be minimum {CategoryValidationConstants.NameMinimumLength} chars length.")
            .MaximumLength(CategoryValidationConstants.NameMaximumLength)
            .WithMessage($"Name is too short. It has to be maximum {CategoryValidationConstants.NameMaximumLength} chars length.");
    }
}
