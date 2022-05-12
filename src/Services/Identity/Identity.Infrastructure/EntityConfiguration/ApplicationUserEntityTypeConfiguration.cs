using System.Net.NetworkInformation;
using Identity.Domain.AggregationModels.ApplicationUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.EntityConfiguration;

public class ApplicationUserEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUserAggregateRoot>
{
    public void Configure(EntityTypeBuilder<ApplicationUserAggregateRoot> builder)
    {
        builder
            .Ignore(user => user.FullName);

        builder
            .Property(user => user.FirstName)
            .IsRequired(true)
            .HasMaxLength(100);
        builder
            .Property(user => user.LastName)
            .IsRequired(true)
            .HasMaxLength(100);

        builder
            .Property(user => user.CreationDate)
            .IsRequired(true);
        
        builder
            .HasOne(u => u.Address)
            .WithOne()
            .IsRequired(false);

        // var navigation = builder.Metadata.FindNavigation(nameof(ApplicationUser.Country));
        // navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
