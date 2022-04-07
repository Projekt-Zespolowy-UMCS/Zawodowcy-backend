using FluentValidation;

namespace Identity.Domain.AggregationModels.ApplicationUser.Child;

public class CountryInfoValidator: AbstractValidator<CountryInfo>
{
    public CountryInfoValidator()
    {
        RuleFor(c => c.Name)
            .NotNull()
            .WithMessage("Country cannot be null.");

        RuleFor(c => c.ISO)
            .NotNull()
            .WithMessage("Country ISO cannot be empty.")
            .Length(CountryInfoEntityValidationConstants.CountryISOCodeLength)
            .WithMessage("Country ISO has to be in three characters length.");
    }
}