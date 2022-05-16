using FluentValidation;

namespace Identity.Domain.AggregationModels.ApplicationUser.Address.CountryInfo;

public class AddressValidator: AbstractValidator<AddressAggregate>
{
    public AddressValidator()
    {
        RuleFor(u => u.City)
            .NotEmpty()
            .WithMessage("City cannot be empty.")
            .MaximumLength(AddressEntityValidationConstants.CityMaxLength)
            .WithMessage($"Maximum length of city name is {AddressEntityValidationConstants.CityMaxLength} characters.")
            .Matches("^[a-zA-Z\\u0080-\\u024F.]+((?:[ -.|'])[a-zA-Z\\u0080-\\u024F]+)*$")
            .WithMessage("There are illegal characters in city name.");

        RuleFor(u => u.Country)
            .SetValidator(new CountryInfoValidator());
        
        RuleFor(u => u.State)
            .NotEmpty()
            .WithMessage("State cannot be empty.")
            .MaximumLength(AddressEntityValidationConstants.StateMaxLength)
            .WithMessage(
                $"State exceeds maximum length of {AddressEntityValidationConstants.StateMaxLength} characters");
        
        RuleFor(u => u.Street)
            .NotEmpty()
            .WithMessage("Street cannot be empty.")
            .MaximumLength(AddressEntityValidationConstants.StreetMaxLength)
            .WithMessage(
                $"Street exceeds maximum length of {AddressEntityValidationConstants.StreetMaxLength} characters");

        RuleFor(u => u.ZipCode)
            .NotEmpty()
            .WithMessage("Zip code cannot be empty.")
            .MaximumLength(AddressEntityValidationConstants.ZipCodeMaxLength)
            .WithMessage(
                $"ZipCode exceeds maximum length of {AddressEntityValidationConstants.ZipCodeMaxLength} characters");
    }
}
