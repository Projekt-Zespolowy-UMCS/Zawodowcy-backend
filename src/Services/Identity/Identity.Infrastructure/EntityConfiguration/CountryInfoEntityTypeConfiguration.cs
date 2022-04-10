using Identity.Domain.AggregationModels.ApplicationUser;
using Identity.Domain.AggregationModels.ApplicationUser.Child;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.EntityConfiguration;

public class CountryInfoEntityTypeConfiguration: IEntityTypeConfiguration<CountryInfo>
{
    public void Configure(EntityTypeBuilder<CountryInfo> builder)
    {
        builder
            .Property(info => info.Name)
            .IsRequired(true);

        builder.Property(info => info.ISO)
            .IsRequired(true)
            .HasMaxLength(3);
    }
}