using System.ComponentModel.Design;
using Identity.Domain.AggregationModels.ApplicationUser;
using Identity.Domain.AggregationModels.ApplicationUser.Child;
using Identity.Infrastructure.EntityConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Data;

public class AppIdentityDbContext: IdentityDbContext<ApplicationUser>
{
    public DbSet<CountryInfo> CountryInfos { get; set; }

    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new ApplicationUserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CountryInfoEntityTypeConfiguration());
        
    }
}