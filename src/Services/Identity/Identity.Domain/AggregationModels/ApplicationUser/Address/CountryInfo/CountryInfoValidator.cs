using FluentValidation;

namespace Identity.Domain.AggregationModels.ApplicationUser.Address.CountryInfo;

public class CountryInfoValidator: AbstractValidator<CountryInfoAggregate>
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
