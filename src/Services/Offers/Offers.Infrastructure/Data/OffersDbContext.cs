using a;
using Microsoft.EntityFrameworkCore;
using Offers.Domain.AggregationModels.Offer;
using Offers.Domain.AggregationModels.Offer.Category;
using Offers.Domain.AggregationModels.Offer.Image;
using Offers.Domain.AggregationModels.Offer.Image.MimeType;
using Offers.Domain.AggregationModels.Offer.Location;

namespace Offers.Infrastructure.Data;

public class OffersDbContext: DbContext
{
    public DbSet<CategoryAggregate> Categories;
    public DbSet<MimeTypeAggregate> MimeTypes;
    public DbSet<LocationAggregate> Locations;
    public DbSet<ImageAggregate> Images;
    public DbSet<OfferAggregateRoot> Offers;

    public OffersDbContext(DbContextOptions<OffersDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new CategoryEntityConfiguration());
        modelBuilder.ApplyConfiguration(new MimeTypeEntityConfiguration());
        modelBuilder.ApplyConfiguration(new LocationEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ImageEntityConfiguration());
        modelBuilder.ApplyConfiguration(new OfferEntityConfiguration());
    }
}
