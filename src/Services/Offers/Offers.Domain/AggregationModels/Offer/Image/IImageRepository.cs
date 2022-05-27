namespace Offers.Domain.AggregationModels.Offer.Image;

public interface IImageRepository
{
    public Task<ImageAggregate> AddImageAsync(ImageAggregate image);
    public Task<IList<ImageAggregate>> GetOfferImagesAsync(Guid offerId);
    public Task<ImageAggregate> UpdateImageAsync(ImageAggregate image);
    public Task<bool> RemoveImageAsync(int id);
}
