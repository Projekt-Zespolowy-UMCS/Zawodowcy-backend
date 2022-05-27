namespace Offers.Domain.AggregationModels.Offer.Category;

public interface ICategoryRepository
{
    public Task<CategoryAggregate> GetCategoryWithNameAsync(string name);
    public Task<IList<CategoryAggregate>> GetAllAsync();
}
