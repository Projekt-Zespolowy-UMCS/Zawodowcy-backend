using System.ComponentModel.DataAnnotations.Schema;
using Identity.Domain.AggregationModels.ApplicationUser.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.AggregationModels.ApplicationUser;

public class ApplicationUser: IdentityUser
{
    public string Street { get; protected set; }
    public string City { get; protected set; }
    public string State { get; protected set; }
    public int CountryId { get; protected set; }
    public CountryInfo Country { get; protected set; }
    public string ZipCode { get; protected set; }
    public string Name { get; protected set; }
    public string LastName { get; protected set; }

    [NotMapped]
    public string FullName
    {
        get => $"{Name} {LastName}";
    }

    public DateTime CreationDate { get; }
    public DateTime LastUpdatedDate { get; protected set; }

    /// <summary>
    /// For Entity Framework purposes
    /// </summary>
    protected ApplicationUser() {}

    public ApplicationUser(string street, string city, string state, CountryInfo country, string zipCode, string name,
        string lastName, string email, string phoneNumber, string userName, string normalizedEmail, string normalizedUserName)
    {
        Street = street ?? throw new ArgumentNullException(nameof(street));
        City = city ?? throw new ArgumentNullException(nameof(city));
        State = state ?? throw new ArgumentNullException(nameof(state));
        Country = country ?? throw new ArgumentNullException(nameof(country));
        ZipCode = zipCode ?? throw new ArgumentNullException(nameof(zipCode));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        CreationDate = new DateTime();
        Email = email ?? throw new ArgumentNullException(nameof(email));
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
        NormalizedEmail = normalizedEmail ?? throw new ArgumentNullException(nameof(normalizedEmail));
        UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        NormalizedEmail = normalizedUserName ?? throw new ArgumentNullException(nameof(normalizedUserName));
    }
}