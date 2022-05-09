using System.Data;
using FluentValidation;
using Identity.Domain.AggregationModels.ApplicationUser.Address.CountryInfo;

namespace Identity.Domain.AggregationModels.ApplicationUser;

public class ApplicationUserValidator: AbstractValidator<ApplicationUserAggregateRoot>
{
    public ApplicationUserValidator()
    {

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
