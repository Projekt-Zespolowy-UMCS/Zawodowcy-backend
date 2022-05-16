using Identity.Application.DTO;
using Identity.Domain.AggregationModels.ApplicationUser.Address.CountryInfo;

namespace Identity.Application.Mappers.UserMapper.CountryInfoMapper;

public interface ICountryInfoMapper
{
    public CountryInfoAggregate MapToEntity(CountryInfoDto dto);
}
