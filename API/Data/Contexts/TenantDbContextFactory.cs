using Microsoft.EntityFrameworkCore;

namespace API.Data.Contexts;

public interface ITenantDbContextFactory
{
    TenantDbContext CreateDbContext(string connectionString);
}

public class TenantDbContextFactory : ITenantDbContextFactory
{
    public TenantDbContext CreateDbContext(string connectionString)
    {
        DbContextOptionsBuilder<TenantDbContext> optionsBuilder = new();
        optionsBuilder.UseNpgsql(connectionString);
        return new TenantDbContext(optionsBuilder.Options);
    }
}
