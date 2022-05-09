using Identity.Application.DTO;
using Identity.Domain.AggregationModels.ApplicationUser.Address.CountryInfo;

namespace Identity.Application.Mappers.UserMapper.CountryInfoMapper;

public class CountryInfoMapper: ICountryInfoMapper
{
    public CountryInfoAggregate MapToEntity(CountryInfoDto dto)
    {
        return new CountryInfoAggregate(dto.ISO, dto.Name);
    }
}
