using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Offers.Domain.AggregationModels.Offer.Image;
using Offers.Infrastructure.Data;

namespace Offers.Infrastructure.Repositories;

public class ImageRepository: IImageRepository
{
    private readonly OffersDbContext _context;

    public ImageRepository(OffersDbContext context)
    {
        _context = context;
    }
    
    public async Task<ImageAggregate> AddImageAsync(ImageAggregate image)
    {
        await _context.Images.AddAsync(image);
        await _context.SaveChangesAsync();

        return image;
    }

    public async Task<IList<ImageAggregate>> GetOfferImagesAsync(Guid offerId)
    {
        return await _context.Images.Where(image => image.OfferId == offerId).ToListAsync();
    }

    public async Task<ImageAggregate> UpdateImageAsync(ImageAggregate image)
    {
        var entity = await _context.Images.FirstAsync(img => img.Id == image.Id);

        _context.Entry(entity).CurrentValues.SetValues(image);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<bool> RemoveImageAsync(Guid id)
    {
        try
        {
            var entity = await _context.Images.FirstAsync(img => img.Id == id);

            _context.Images.Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }
}
