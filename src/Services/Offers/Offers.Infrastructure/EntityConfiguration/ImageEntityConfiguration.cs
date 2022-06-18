using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Offers.Domain.AggregationModels.Offer.Image;

namespace a;

public class ImageEntityConfiguration: IEntityTypeConfiguration<ImageAggregate>
{
    public void Configure(EntityTypeBuilder<ImageAggregate> builder)
    {
        builder.ToTable("Image");

        builder
            .Property(image => image.Name)
            .IsRequired()
            .HasMaxLength(ImageValidationConstants.NameMaximumLength);

        builder
            .HasOne(image => image.MimeType)
            .WithMany();

        builder
            .HasOne(image => image.Offer)
            .WithOne()
            .HasForeignKey<ImageAggregate>(image => image.OfferId);
        
    }
}
