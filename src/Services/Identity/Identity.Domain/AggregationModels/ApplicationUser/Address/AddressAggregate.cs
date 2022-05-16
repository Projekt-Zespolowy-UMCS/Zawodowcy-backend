using FluentValidation;
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
    
    protected AddressAggregate() {}

    public AddressAggregate(string street, string city, string state, CountryInfoAggregate country, string zipCode)
    {
        Street = street ?? throw new ArgumentException(nameof(street));
        City = city ?? throw new ArgumentException(nameof(city));
        State = state  ?? throw new ArgumentException(nameof(state));
        Country = country ?? throw new ArgumentException(nameof(country));
        ZipCode = zipCode  ?? throw new ArgumentException(nameof(zipCode));
        
        new AddressValidator().ValidateAndThrow<AddressAggregate>(this);
    }
    
    public AddressAggregate(int id, string street, string city, string state, CountryInfoAggregate country, string 
    zipCode)
    {
        Id = id == 0 ? throw new ArgumentException(nameof(id)) : id;
        Street = street ?? throw new ArgumentException(nameof(street));
        City = city ?? throw new ArgumentException(nameof(city));
        State = state  ?? throw new ArgumentException(nameof(state));
        Country = country ?? throw new ArgumentException(nameof(country));
        ZipCode = zipCode  ?? throw new ArgumentException(nameof(zipCode));
        
        new AddressValidator().ValidateAndThrow<AddressAggregate>(this);
    }
}
