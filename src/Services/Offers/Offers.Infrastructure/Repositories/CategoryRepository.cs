using Microsoft.EntityFrameworkCore;
using Offers.Domain.AggregationModels.Offer.Category;
using Offers.Infrastructure.Data;

namespace Offers.Infrastructure.Repositories;

public class CategoryRepository: ICategoryRepository
{
    private readonly OffersDbContext _context;

    public CategoryRepository(OffersDbContext context)
    {
        _context = context;
    }
    
    public async Task<CategoryAggregate> GetCategoryWithNameAsync(string name)
    {
        return await _context.Categories.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<IList<CategoryAggregate>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }
}
