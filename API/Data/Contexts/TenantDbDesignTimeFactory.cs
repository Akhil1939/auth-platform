using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace API.Data.Contexts;

public class TenantDbDesignTimeFactory : IDesignTimeDbContextFactory<TenantDbContext>
{
    public TenantDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();

        // Set your local dev connection string here
        var connectionString = "Host=localhost;Port=5432;Database=tenant_dev;Username=postgres;Password=admin";
        optionsBuilder.UseNpgsql(connectionString);

        return new TenantDbContext(optionsBuilder.Options);
    }
}
