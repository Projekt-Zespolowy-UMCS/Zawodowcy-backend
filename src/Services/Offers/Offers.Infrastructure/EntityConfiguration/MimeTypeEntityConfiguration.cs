using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Offers.Domain.AggregationModels.Offer.Category;
using Offers.Domain.AggregationModels.Offer.Image.MimeType;

namespace a;

public class MimeTypeEntityConfiguration: IEntityTypeConfiguration<MimeTypeAggregate>
{
    public void Configure(EntityTypeBuilder<MimeTypeAggregate> builder)
    {
        builder.ToTable("MimeType");
    }
}
