using FluentValidation;

namespace Identity.Domain.AggregationModels.ApplicationUser.ValueObjects;

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
            .WithMessage("Country ISO has to be in three characters length.")
            .Matches("^[A-Z]{3}$")
            .WithMessage("Country ISO is invalid.");

    }
}