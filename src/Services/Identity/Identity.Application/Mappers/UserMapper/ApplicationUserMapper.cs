using Identity.Application.DTO;
using Identity.Application.DTO.RegisteringUser;
using Identity.Domain.AggregationModels.ApplicationUser;
using Identity.Domain.AggregationModels.ApplicationUser.Child;

namespace Identity.Application.Mappers.UserMapper;

public class ApplicationUserMapper: IMapper<ApplicationUser, RegisterApplicationUserDto>
{
    public readonly IMapper<CountryInfo, CountryInfoDto> _countryMapper;

    public ApplicationUserMapper(IMapper<CountryInfo, CountryInfoDto> countryMapper)
    {
        _countryMapper = countryMapper;
    }
    
    public ApplicationUser MapToEntity(RegisterApplicationUserDto dto)
    {
        var countryInfo = _countryMapper.MapToEntity(dto.CountryInfo);
        
        return new ApplicationUser(
            dto.Street, dto.City, dto.City, countryInfo, dto.ZipCode, dto.FirstName,
            dto.LastName, dto.Email, dto.PhoneNumber, dto.UserName);
    }

}
