using Identity.Application.DTO;
using Identity.Domain.AggregationModels.ApplicationUser.Child;

namespace Identity.Application.Mappers;

public class CountryInfoMapper: IMapper<CountryInfo, CountryInfoDto>
{
    public CountryInfo MapToEntity(CountryInfoDto dto)
    {
        return new CountryInfo(dto.Name, dto.ISO);
    }
}
