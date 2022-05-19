using AutoMapper;
using Identity.Application.DTO;
using Identity.Domain.AggregationModels.ApplicationUser.Address.CountryInfo;

namespace Identity.Api.Configuration;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CountryInfoAggregate, CountryInfoDto>();
    }
}
