using Identity.Application.DTO.Address;
using Identity.Application.Mappers.UserMapper.CountryInfoMapper;
using Identity.Domain.AggregationModels.ApplicationUser.Address;
using Identity.Domain.AggregationModels.ApplicationUser.Address.CountryInfo;

namespace Identity.Application.Mappers.UserMapper.AddressMapper;

public class AddressMapper: IAddressMapper
{
    private readonly ICountryInfoMapper _countryInfoMapper;
    
    public AddressMapper(ICountryInfoMapper countryInfoMapper)
    {
        _countryInfoMapper = countryInfoMapper;
    }
    
    public AddressAggregate MapToEntity(AddAddressDto dto)
    {
        var countryInfo = _countryInfoMapper.MapToEntity(dto.Country);

        return new AddressAggregate(dto.Street, dto.City, dto.State, countryInfo, dto.ZipCode);
    }

    public AddressAggregate MapToEntity(UpdateAddressDto dto)
    {
        var countryInfo = _countryInfoMapper.MapToEntity(dto.Country);

        return new AddressAggregate(dto.Street, dto.City, dto.State, countryInfo, dto.ZipCode);
    }
}
