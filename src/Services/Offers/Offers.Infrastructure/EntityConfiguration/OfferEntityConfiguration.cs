using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Offers.Domain.AggregationModels.Offer;
using Offers.Domain.AggregationModels.Offer.Image.MimeType;

namespace a;

public class OfferEntityConfiguration: IEntityTypeConfiguration<OfferAggregateRoot>
{
    public void Configure(EntityTypeBuilder<OfferAggregateRoot> builder)
    {
        builder.ToTable("Offer");
        
        ConfigureProperties(builder);
        ConfigureRelationships(builder);
    }

    private void ConfigureProperties(EntityTypeBuilder<OfferAggregateRoot> builder)
    {
        builder
            .Property(offer => offer.Content)
            .IsRequired()
            .HasMaxLength(OfferValidationConstants.ContentMaximumLength);

        builder
            .Property(offer => offer.Title)
            .IsRequired()
            .HasMaxLength(OfferValidationConstants.TitleMaximumLength);
    }

    private void ConfigureRelationships(EntityTypeBuilder<OfferAggregateRoot> builder)
    {
        builder
            .HasOne(offer => offer.Category)
            .WithMany()
            .HasForeignKey(offer => offer.CategoryId);

        builder
            .HasOne(offer => offer.Location)
            .WithMany()
            .HasForeignKey(offer => offer.LocationId);

        builder
            .HasMany(offer => offer.Images)
            .WithOne();
    }
}
