using FluentValidation;

namespace Offers.Domain.AggregationModels.Offer.Location;

public class LocationValidator: AbstractValidator<LocationAggregate>
{
    public LocationValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(LocationValidationConstants.NameMinimumLength)
            .WithMessage($"Name is too short. It has to be minimum {LocationValidationConstants.NameMinimumLength} chars length.")
            .MaximumLength(LocationValidationConstants.NameMaximumLength)
            .WithMessage($"Name is too short. It has to be maximum {LocationValidationConstants.NameMaximumLength} chars length.");
        
        RuleFor(x => x.CountryISO)
            .NotNull()
            .WithMessage("Country ISO cannot be empty.")
            .Length(LocationValidationConstants.CountryISOCodeLength)
            .WithMessage("Country ISO has to be in three characters length.");
    }
}
