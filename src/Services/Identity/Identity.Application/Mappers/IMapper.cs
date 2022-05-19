namespace Identity.Application.Mappers;

public interface IMapper<TEntity, TDto>
{
    public TEntity MapToEntity(TDto dto);
}
