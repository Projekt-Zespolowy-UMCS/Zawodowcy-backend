using System.ComponentModel.Design;
using Identity.Domain.AggregationModels.ApplicationUser;
using Identity.Domain.AggregationModels.ApplicationUser.Address;
using Identity.Domain.AggregationModels.ApplicationUser.Address.CountryInfo;
using Identity.Infrastructure.EntityConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data;

public class AppIdentityDbContext: IdentityDbContext<ApplicationUserAggregateRoot>
{
    public DbSet<CountryInfoAggregate> CountryInfos { get; set; }
    
    public DbSet<AddressAggregate> Addresses { get; set; }

    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new ApplicationUserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AddressEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CountryInfoEntityTypeConfiguration());
        
    }
}
