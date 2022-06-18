namespace Offers.Domain.AggregationModels.Offer.Image.MimeType;

public interface IMimeTypeRepository
{
    public Task<MimeTypeAggregate> AddMimeTypeAsync(MimeTypeAggregate mimeType);
    public Task<IList<MimeTypeAggregate>> GetAllAsync();
}
