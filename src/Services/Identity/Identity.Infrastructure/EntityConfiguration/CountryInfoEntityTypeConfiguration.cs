using Identity.Domain.AggregationModels.ApplicationUser;
using Identity.Domain.AggregationModels.ApplicationUser.Address.CountryInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.EntityConfiguration;

public class CountryInfoEntityTypeConfiguration: IEntityTypeConfiguration<CountryInfoAggregate>
{
    public void Configure(EntityTypeBuilder<CountryInfoAggregate> builder)
    {
        builder
            .Property(info => info.Name)
            .IsRequired(true);

        builder.Property(info => info.ISO)
            .IsRequired(true)
            .HasMaxLength(3);
    }
}
