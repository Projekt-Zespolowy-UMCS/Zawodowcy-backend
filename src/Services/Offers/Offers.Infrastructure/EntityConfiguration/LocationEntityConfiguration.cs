using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Offers.Domain.AggregationModels.Offer.Image;
using Offers.Domain.AggregationModels.Offer.Location;

namespace a;

public class LocationEntityConfiguration: IEntityTypeConfiguration<LocationAggregate>
{
    public void Configure(EntityTypeBuilder<LocationAggregate> builder)
    {
        builder.ToTable("Location");

        builder
            .Property(location => location.Name)
            .IsRequired()
            .HasMaxLength(LocationValidationConstants.NameMaximumLength);

        builder
            .Property(location => location.CountryISO)
            .IsRequired()
            .HasMaxLength(LocationValidationConstants.CountryISOCodeLength);

    }
}
