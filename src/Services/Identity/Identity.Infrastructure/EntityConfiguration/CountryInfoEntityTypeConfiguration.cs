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
            .HasKey(info => info.ISO);
        
        builder.Property(info => info.ISO)
            .IsRequired()
            .HasMaxLength(3);
        
        builder
            .Property(info => info.Name)
            .IsRequired();
    }
}
