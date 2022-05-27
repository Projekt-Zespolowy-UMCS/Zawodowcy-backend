using Offers.Domain.AggregationModels.Offer.Image;
using Offers.Domain.Base.Pagination;

namespace Offers.Domain.AggregationModels.Offer;

public interface IOfferRepository
{
    public Task<OfferAggregateRoot> AddAsync(OfferAggregateRoot offer);
    public Task<OfferAggregateRoot> UpdateAsync(OfferAggregateRoot offer);
    public Task<bool> RemoveAsync(Guid offerId);
    public Task<IList<OfferAggregateRoot>> GetOffersWithCategory(int categoryId);
    public Task<Pager<OfferAggregateRoot>> GetAll(PageConfiguration config);
}
