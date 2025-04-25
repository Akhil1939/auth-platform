namespace API.Data.Contexts;

using API.Data.Models.Tenant;
using Microsoft.EntityFrameworkCore;

public class CentralDbContext : DbContext
{
    public CentralDbContext(DbContextOptions<CentralDbContext> options) : base(options) { }

    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<CustomField> CustomFields { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Tenant>().HasIndex(t => t.Slug).IsUnique();
    }
}
