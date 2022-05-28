using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Offers.Domain.AggregationModels.Offer.Category;

namespace a;

public class CategoryEntityConfiguration: IEntityTypeConfiguration<CategoryAggregate>
{
    public void Configure(EntityTypeBuilder<CategoryAggregate> builder)
    {
        builder.ToTable("Category");
        
        builder
            .Property(category => category.Name)
            .IsRequired(true)
            .HasMaxLength(CategoryValidationConstants.NameMaximumLength);


        // var navigation = builder.Metadata.FindNavigation(nameof(ApplicationUser.Country));
        // navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
