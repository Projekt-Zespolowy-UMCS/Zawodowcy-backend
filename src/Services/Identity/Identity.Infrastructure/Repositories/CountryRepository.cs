
using Identity.Domain.AggregationModels.ApplicationUser.Address.CountryInfo;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

public class CountryRepository : ICountryRepository
{
    private readonly AppIdentityDbContext _context;
    
    public CountryRepository(AppIdentityDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<CountryInfoAggregate>> GetAllAsync()
    {
        return await _context.CountryInfos.ToListAsync();
    }

    public async Task<CountryInfoAggregate?> GetAsync(int id)
    {
        return await _context.CountryInfos.FirstOrDefaultAsync(ctr => ctr.Id == id);
    }

    public async Task<CountryInfoAggregate?> GetAsync(string iso)
    {
        return await _context.CountryInfos.FirstOrDefaultAsync(ctr => ctr.ISO == iso);

    }
}
