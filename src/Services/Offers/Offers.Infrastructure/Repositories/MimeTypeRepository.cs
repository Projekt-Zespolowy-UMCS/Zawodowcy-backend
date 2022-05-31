using Microsoft.EntityFrameworkCore;
using Offers.Domain.AggregationModels.Offer.Image.MimeType;
using Offers.Infrastructure.Data;

namespace Offers.Infrastructure.Repositories;

public class MimeTypeRepository: IMimeTypeRepository
{
    private readonly OffersDbContext _context;

    public MimeTypeRepository(OffersDbContext context)
    {
        _context = context;
    }
    
    public async Task<MimeTypeAggregate> AddMimeTypeAsync(MimeTypeAggregate mimeType)
    {
        await _context.MimeTypes.AddAsync(mimeType);
        await _context.SaveChangesAsync();

        return mimeType;
    }

    public async Task<IList<MimeTypeAggregate>> GetAllAsync()
    {
        return await _context.MimeTypes.ToListAsync();
    }
}
