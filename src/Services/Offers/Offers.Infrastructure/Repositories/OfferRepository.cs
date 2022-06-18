using Microsoft.EntityFrameworkCore;
using Offers.Domain.AggregationModels.Offer;
using Offers.Domain.Base.Pagination;
using Offers.Infrastructure.Data;

namespace Offers.Infrastructure.Repositories;

public class OfferRepository: IOfferRepository
{
    private readonly OffersDbContext _context;

    public OfferRepository(OffersDbContext context)
    {
        _context = context;
    }
    
    public async Task<OfferAggregateRoot> AddAsync(OfferAggregateRoot offer)
    {
        await _context.Offers.AddAsync(offer);
        await _context.SaveChangesAsync();
        return offer;
    }

    public async Task<OfferAggregateRoot> UpdateAsync(OfferAggregateRoot offer)
    {
        var entity = await _context.Offers.FirstAsync(ofr => ofr.Id == offer.Id);

        _context.Entry(entity).CurrentValues.SetValues(offer);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<bool> RemoveAsync(Guid offerId)
    {
        try
        {
            var entity = await _context.Offers.FirstAsync(ofr => ofr.Id == offerId);

            _context.Offers.Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }

    public async Task<IList<OfferAggregateRoot>> GetOffersWithCategory(int categoryId)
    {
        return await _context.Offers
            .Include(offer => offer.Category)
            .Where(offer => offer.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<Pager<OfferAggregateRoot>> GetAll(PageConfiguration config)
    {
        var items = await _context.Offers
            .Skip(config.PageSize*config.Page)
            .Take(config.PageSize)
            .ToListAsync();

        var itemsCount = await _context.Offers.CountAsync();

        return new Pager<OfferAggregateRoot>(
            itemsCount,
            config.Page,
            config.PageSize,
            items);
    }
}
