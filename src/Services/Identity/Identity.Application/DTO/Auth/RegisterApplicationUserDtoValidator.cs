using FluentValidation;
using Identity.Domain.AggregationModels.ApplicationUser;
using Identity.Domain.AggregationModels.ApplicationUser.ValueObjects;

namespace Identity.Application.DTO.Auth;

public class RegisterApplicationUserDtoValidator: AbstractValidator<ApplicationUser>
{
    public RegisterApplicationUserDtoValidator()
    {
        RuleFor(u => u.City)
            .NotEmpty()
            .WithMessage("City cannot be empty.")
            .MaximumLength(ApplicationUserEntityValidationConstants.CityMaxLength)
            .WithMessage($"Maximum length of city name is {ApplicationUserEntityValidationConstants.CityMaxLength} characters.")
            .Matches("^[a-zA-Z\\u{0080}-\\u{024F}\\s\\/\\-\\)\\(\\`\\.\\\"\\']*$")
            .WithMessage("There are illegal characters in city name.");

        RuleFor(u => u.Country)
            .NotNull()
            .WithMessage("Country cannot be null.")
            .SetValidator(new CountryInfoValidator());

        RuleFor(u => u.FirstName)
            .NotEmpty()
            .WithMessage("First name cannot be empty.")
            .MaximumLength(ApplicationUserEntityValidationConstants.FirstNameMaxLength)
            .WithMessage(
                $"First name exceeds maximum length of {ApplicationUserEntityValidationConstants.FirstNameMaxLength} characters.");

        RuleFor(u => u.LastName)
            .NotEmpty()
            .WithMessage($"Last name cannot be empty.")
            .MaximumLength(ApplicationUserEntityValidationConstants.FirstNameMaxLength)
            .WithMessage(
                $"Last name exceeds maximum length of {ApplicationUserEntityValidationConstants.FirstNameMaxLength} characters.");

        RuleFor(u => u.State)
            .NotEmpty()
            .WithMessage("State cannot be empty.")
            .MaximumLength(ApplicationUserEntityValidationConstants.StateMaxLength)
            .WithMessage(
                $"State exceeds maximum length of {ApplicationUserEntityValidationConstants.StateMaxLength} characters");
        
        RuleFor(u => u.Street)
            .NotEmpty()
            .WithMessage("Street cannot be empty.")
            .MaximumLength(ApplicationUserEntityValidationConstants.StreetMaxLength)
            .WithMessage(
                $"Street exceeds maximum length of {ApplicationUserEntityValidationConstants.StreetMaxLength} characters");

        RuleFor(u => u.ZipCode)
            .NotEmpty()
            .WithMessage("Zip code cannot be empty.")
            .MaximumLength(ApplicationUserEntityValidationConstants.ZipCodeMaxLength)
            .WithMessage(
                $"ZipCode exceeds maximum length of {ApplicationUserEntityValidationConstants.ZipCodeMaxLength} characters");

        RuleFor(u => u.CreationDate)
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("Creation date is invalid. It cannot be set in the future.");

        When(u => u.LastUpdatedDate != null, () =>
        {
            RuleFor(u => u.CreationDate)
                .LessThanOrEqualTo(u => u.LastUpdatedDate)
                .WithMessage("Last update date is invalid. It cannot be set before Creation date.");
        });
    }
}
