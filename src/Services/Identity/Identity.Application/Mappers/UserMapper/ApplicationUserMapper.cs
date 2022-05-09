using AutoMapper;
using Identity.Application.DTO;
using Identity.Application.DTO.RegisteringUser;
using Identity.Application.Mappers.UserMapper.AddressMapper;
using Identity.Application.Mappers.UserMapper.CountryInfoMapper;
using Identity.Domain.AggregationModels.ApplicationUser;
using Identity.Domain.AggregationModels.ApplicationUser.Address.CountryInfo;

namespace Identity.Application.Mappers.UserMapper;

public class ApplicationUserMapper: IApplicationUserMapper
{
    public readonly IAddressMapper _addressMapper;

    public ApplicationUserMapper(IAddressMapper addressMapper)
    {
        _addressMapper = addressMapper;
    }
    
    public ApplicationUserAggregateRoot MapToEntity(RegisterApplicationUserDto dto)
    {
        // var countryInfo = _countryMapper.MapToEntity(dto.CountryInfo);
        return new ApplicationUserAggregateRoot(dto.FirstName, dto.LastName, dto.Email, dto.PhoneNumber, null);
    }

}
