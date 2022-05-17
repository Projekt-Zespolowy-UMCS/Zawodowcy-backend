using Identity.Domain.AggregationModels.ApplicationUser.Address;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.EntityConfiguration;

public class AddressEntityTypeConfiguration: IEntityTypeConfiguration<AddressAggregate>
{
    public void Configure(EntityTypeBuilder<AddressAggregate> builder)
    {
        builder
            .Property(address => address.City)
            .IsRequired(true)
            .HasMaxLength(AddressEntityValidationConstants.CityMaxLength);

        builder
            .Property(address => address.State)
            .IsRequired(true)
            .HasMaxLength(AddressEntityValidationConstants.StateMaxLength);

        builder
            .Property(address => address.Street)
            .HasMaxLength(AddressEntityValidationConstants.StreetMaxLength);

        builder
            .Property(address => address.ZipCode)
            .IsRequired(true)
            .HasMaxLength(40);

        builder
            .HasOne(address => address.Country)
            .WithMany()
            .HasForeignKey(x => x.CountryId);

        // var navigation = builder.Metadata.FindNavigation(nameof(ApplicationUser.Country));
        // navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
