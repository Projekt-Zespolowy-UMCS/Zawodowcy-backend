using Identity.Application.DTO.Address;
using Identity.Domain.AggregationModels.ApplicationUser.Address;

namespace Identity.Application.Mappers.UserMapper.AddressMapper;

public interface IAddressMapper
{
    public AddressAggregate MapToEntity(AddAddressDto dto);
    public AddressAggregate MapToEntity(UpdateAddressDto dto);
}
