using Microsoft.EntityFrameworkCore;
using Offers.Domain.AggregationModels.Offer.Location;
using Offers.Domain.Base.Pagination;
using Offers.Infrastructure.Data;

namespace Offers.Infrastructure.Repositories;

public class LocationRepository: ILocationRepository
{
    private readonly OffersDbContext _context;

    public LocationRepository(OffersDbContext context)
    {
        _context = context;
    }
    
    public async Task<LocationAggregate> AddLocationAsync(LocationAggregate location)
    {
        await _context.Locations.AddAsync(location);
        await _context.SaveChangesAsync();

        return location;
    }

    public async Task<IList<LocationAggregate>> GetAllLocationsAsync(PageConfiguration config)
    {
        return await _context.Locations
            .Skip(config.PageSize*config.Page)
            .Take(config.PageSize)
            .ToListAsync();
    }

    public async Task<IList<LocationAggregate>> GetLocationsWithNameAsync(string name)
    {
        return await _context.Locations
            .Where(x => x.Name.ToLower().Contains(name.ToLower()))
            .ToListAsync();
    }
}
