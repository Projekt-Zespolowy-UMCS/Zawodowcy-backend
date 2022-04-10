using AutoMapper;
using Identity.Application.DTO;
using Identity.Application.DTO.RegisteringUser;
using Identity.Domain.AggregationModels.ApplicationUser;
using Identity.Domain.AggregationModels.ApplicationUser.Child;

namespace Identity.Application.Mappers.UserMapper;

public class ApplicationUserMapper: IMapper<ApplicationUser, RegisterApplicationUserDto>
{
    public readonly IMapper _mapper;

    public ApplicationUserMapper(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    public ApplicationUser MapToEntity(RegisterApplicationUserDto dto)
    {
        // var countryInfo = _countryMapper.MapToEntity(dto.CountryInfo);
        var countryInfo = _mapper.Map<CountryInfo>(dto.CountryInfo);
        return new ApplicationUser(
            dto.Street, dto.City, dto.City, countryInfo, dto.ZipCode, dto.FirstName,
            dto.LastName, dto.Email, dto.PhoneNumber, dto.UserName);
    }

}
