using Identity.Application.DTO.RegisteringUser;
using Identity.Domain.AggregationModels.ApplicationUser;

namespace Identity.Application.Mappers.UserMapper;

public interface IApplicationUserMapper
{
    public ApplicationUserAggregateRoot MapToEntity(RegisterApplicationUserDto dto);
}
