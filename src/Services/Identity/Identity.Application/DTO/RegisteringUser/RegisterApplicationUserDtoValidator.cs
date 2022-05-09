using FluentValidation;
using Identity.Domain.AggregationModels.ApplicationUser;

namespace Identity.Application.DTO.RegisteringUser;

public class RegisterApplicationUserDtoValidator: AbstractValidator<RegisterApplicationUserDto>
{
    public RegisterApplicationUserDtoValidator()
    {
        // RuleFor(u => u.City)
        //     .NotEmpty()
        //     .WithMessage("City cannot be empty.")
        //     .MaximumLength(ApplicationUserEntityValidationConstants.CityMaxLength)
        //     .WithMessage($"Maximum length of city name is {ApplicationUserEntityValidationConstants.CityMaxLength} characters.")
        //     .Matches("^[a-zA-Z\\u{0080}-\\u{024F}\\s\\/\\-\\)\\(\\`\\.\\\"\\']*$")
        //     .WithMessage("There are illegal characters in city name.");
        //
        // RuleFor(u => u.CountryInfo)
        //     .NotNull()
        //     .WithMessage("Country cannot be null.")
        //     .SetValidator(new CountryInfoDtoValidator());

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

        // RuleFor(u => u.State)
        //     .NotEmpty()
        //     .WithMessage("State cannot be empty.")
        //     .MaximumLength(ApplicationUserEntityValidationConstants.StateMaxLength)
        //     .WithMessage(
        //         $"State exceeds maximum length of {ApplicationUserEntityValidationConstants.StateMaxLength} characters");
        //
        // RuleFor(u => u.Street)
        //     .NotEmpty()
        //     .WithMessage("Street cannot be empty.")
        //     .MaximumLength(ApplicationUserEntityValidationConstants.StreetMaxLength)
        //     .WithMessage(
        //         $"Street exceeds maximum length of {ApplicationUserEntityValidationConstants.StreetMaxLength} characters");
        //
        // RuleFor(u => u.ZipCode)
        //     .NotEmpty()
        //     .WithMessage("Zip code cannot be empty.")
        //     .MaximumLength(ApplicationUserEntityValidationConstants.ZipCodeMaxLength)
        //     .WithMessage(
        //         $"ZipCode exceeds maximum length of {ApplicationUserEntityValidationConstants.ZipCodeMaxLength} characters");
    }
}
