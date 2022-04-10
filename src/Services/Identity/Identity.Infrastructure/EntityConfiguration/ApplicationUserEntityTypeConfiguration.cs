using System.Net.NetworkInformation;
using Identity.Domain.AggregationModels.ApplicationUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.EntityConfiguration;

public class ApplicationUserEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .Ignore(user => user.FullName);

        builder
            .Property(user => user.City)
            .IsRequired(true)
            .HasMaxLength(100);

        builder
            .Property(user => user.FirstName)
            .IsRequired(true)
            .HasMaxLength(100);
        builder
            .Property(user => user.LastName)
            .IsRequired(true)
            .HasMaxLength(100);

        builder
            .Property(user => user.State)
            .IsRequired(true)
            .HasMaxLength(100);

        builder
            .Property(user => user.Street)
            .HasMaxLength(100);

        builder
            .Property(user => user.ZipCode)
            .IsRequired(true)
            .HasMaxLength(40);

        builder
            .Property(user => user.CreationDate)
            .IsRequired(true);

        builder
            .HasOne(user => user.Country)
            .WithOne()
            .IsRequired(true);
        

        // var navigation = builder.Metadata.FindNavigation(nameof(ApplicationUser.Country));
        // navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}