using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Mappers.UserMapper;

public interface IUserMapper<TEntity, TDto>: IMapper<TEntity, TDto> where TEntity : IdentityUser where TDto: new()
{
    public TEntity MapToEntity(TDto dto);
}


