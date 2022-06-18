using Offers.Domain.Base.Pagination;

namespace Offers.Domain.AggregationModels.Offer.Location;

public interface ILocationRepository
{
    public Task<LocationAggregate> AddLocationAsync(LocationAggregate location);
    public Task<Pager<LocationAggregate>> GetAllLocationsAsync(PageConfiguration config);
    public Task<IList<LocationAggregate>> GetLocationsWithNameAsync(string name);
}
