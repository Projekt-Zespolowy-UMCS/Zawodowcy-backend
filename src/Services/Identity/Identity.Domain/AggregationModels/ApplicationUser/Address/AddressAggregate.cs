using Identity.Domain.AggregationModels.ApplicationUser.Address.CountryInfo;
using Identity.Domain.Base;

namespace Identity.Domain.AggregationModels.ApplicationUser.Address;

public class AddressAggregate: BaseEntity
{
    public string Street { get; protected set; }
    public string City { get; protected set; }
    public string State { get; protected set; }
    public int CountryId { get; protected set; }
    public CountryInfoAggregate Country { get; protected set; }
    public string ZipCode { get; protected set; }

    public AddressAggregate(string street, string city, string state, CountryInfoAggregate country, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        Country = country;
        ZipCode = zipCode;
    }
}
