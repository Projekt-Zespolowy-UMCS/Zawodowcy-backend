using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Identity.Domain.AggregationModels.ApplicationUser.Address;
using Identity.Domain.AggregationModels.ApplicationUser.Address.CountryInfo;
using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.AggregationModels.ApplicationUser;

public class ApplicationUserAggregateRoot: IdentityUser
{
    public int AddressId { get; protected set; }
    public AddressAggregate? Address { get; protected set; }
    public string FirstName { get; protected set; }
    public string LastName { get; protected set; }

    [NotMapped]
    public string FullName
    {
        get => $"{FirstName} {LastName}";
    }

    public DateTime CreationDate { get; }
    public DateTime? LastUpdatedDate { get; protected set; }

    /// <summary>
    /// For Entity Framework purposes
    /// </summary>
    protected ApplicationUserAggregateRoot() {}

    public ApplicationUserAggregateRoot(string firstName, string lastName, string email, string phoneNumber, AddressAggregate? address)
    {
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        CreationDate = new DateTime();
        Email = email ?? throw new ArgumentNullException(nameof(email));
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
        UserName = email ?? throw new ArgumentNullException(nameof(email));
        address = Address;

        new ApplicationUserValidator().ValidateAndThrow<ApplicationUserAggregateRoot>(this);
    }

    public void SetUserAddress(AddressAggregate address)
    {
        Address = address;
        AddressId = address.Id;
    }
}
