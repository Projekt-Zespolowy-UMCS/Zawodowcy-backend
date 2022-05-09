using FluentValidation;
using Identity.Domain.AggregationModels.ApplicationUser;
using Identity.Domain.AggregationModels.ApplicationUser.Address.CountryInfo;

namespace Identity.Application.DTO;

public class CountryInfoDtoValidator: AbstractValidator<CountryInfoDto>
{
    public CountryInfoDtoValidator()
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
