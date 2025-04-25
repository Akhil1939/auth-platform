using API.Data.Contexts;
using API.Data.Models.Central;
using API.Data.Models.Tenant;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class MigrationUpdateService
{
    private readonly CentralDbContext _centralDb;
    private readonly ITenantDbContextFactory _dbContextFactory;

    public MigrationUpdateService(CentralDbContext centralDb, ITenantDbContextFactory dbContextFactory)
    {
        _centralDb = centralDb;
        _dbContextFactory = dbContextFactory;
    }
    public async Task<int> ApplyMigrationsToAllTenantsAsync()
    {
        List<Tenant> tenants = await _centralDb.Tenants.ToListAsync();
        int updated = 0;

        foreach (Tenant tenant in tenants)
        {
            try
            {
                using TenantDbContext tenantDb = _dbContextFactory.CreateDbContext(tenant.DbConnectionString);
                await tenantDb.Database.MigrateAsync();

                IEnumerable<string> latest = await tenantDb.Database.GetAppliedMigrationsAsync();
                string last = latest.LastOrDefault() ?? "None";

                MigrationStatus? status = await _centralDb.MigrationStatuses
                    .FirstOrDefaultAsync(m => m.TenantId == tenant.Id);

                if (status == null)
                {
                    _centralDb.MigrationStatuses.Add(new MigrationStatus
                    {
                        TenantId = tenant.Id,
                        LastMigration = last
                    });
                }
                else
                {
                    status.LastMigration = last;
                    status.LastChecked = DateTime.UtcNow;
                }

                updated++;
            }
            catch (Exception ex)
            {
                // Log error per tenant if needed
                Console.WriteLine($"Migration failed for {tenant.Slug}: {ex.Message}");
            }
        }

        await _centralDb.SaveChangesAsync();
        return updated;
    }
}