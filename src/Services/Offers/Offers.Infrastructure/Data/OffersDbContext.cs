using a;
using Microsoft.EntityFrameworkCore;

namespace Offers.Infrastructure.Data;

public class OffersDbContext: DbContext
{
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
