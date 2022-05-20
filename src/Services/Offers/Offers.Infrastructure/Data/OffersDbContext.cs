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
        
    }
}
