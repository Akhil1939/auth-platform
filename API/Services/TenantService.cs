using API.Data.Contexts;
using API.Data.DTOs;
using API.Data.Models.Central;
using API.Data.Models.Tenant;
using API.Utils;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class TenantService
{
    private readonly CentralDbContext _centralDb;
    private readonly ITenantDbContextFactory _dbContextFactory;
    private readonly IConfiguration _config;

    public TenantService(CentralDbContext centralDb, ITenantDbContextFactory dbContextFactory, IConfiguration config)
    {
        _centralDb = centralDb;
        _dbContextFactory = dbContextFactory;
        _config = config;
    }

    public async Task<Tenant> RegisterTenantAsync(TenantRegistrationRequest request)
    {
        // Create DB connection string dynamically
        string dbName = $"tenant_{request.Slug}";
        string? connTemplate = _config.GetConnectionString("TenantDbTemplate");
        string connStr = connTemplate?.Replace("{DB_NAME}", dbName);

        // Save tenant in central DB
        Tenant tenant = new()
        {
            Name = request.Name,
            Slug = request.Slug,
            DbConnectionString = connStr,
            BrandingJson = "temp",
            CreatedAt = DateTime.UtcNow,
        };
        _centralDb.Tenants.Add(tenant);
        //await _centralDb.SaveChangesAsync();

        // Create tenant DB
        await CreateTenantDatabaseAsync(dbName);

        // Migrate schema
        using TenantDbContext tenantDb = _dbContextFactory.CreateDbContext(connStr);
        await tenantDb.Database.MigrateAsync();

        // Track latest applied migration
        IEnumerable<string> appliedMigrations = await tenantDb.Database.GetAppliedMigrationsAsync();
        string lastMigration = appliedMigrations.LastOrDefault() ?? "None";

        _centralDb.MigrationStatuses.Add(new MigrationStatus
        {
            TenantId = tenant.Id,
            LastMigration = lastMigration,
            LastChecked = DateTime.UtcNow
        });
        await _centralDb.SaveChangesAsync();

        // Create admin user (optional)
        (string hash, string salt) = PasswordHelper.CreatePasswordHash(request.AdminPassword);
        TenantUser adminUser = new()
        {
            Email = request.AdminEmail,
            Password = hash,
            Salt = salt,
            UserRoles =
            [
                new TenantUserRole
                {
                    RoleId = 1, // Admin role ID
                }
            ],
        };
        tenantDb.Users.Add(adminUser);
        await tenantDb.SaveChangesAsync();

        return tenant;
    }

    private async Task CreateTenantDatabaseAsync(string dbName)
    {
        string? adminConn = _config.GetConnectionString("PostgresAdmin");
        Npgsql.NpgsqlConnectionStringBuilder builder = new(adminConn);
        using Npgsql.NpgsqlConnection conn = new(adminConn);
        await conn.OpenAsync();

        using Npgsql.NpgsqlCommand cmd = new($"CREATE DATABASE \"{dbName}\";", conn);
        await cmd.ExecuteNonQueryAsync();
    }
}

