using API.Data.Configurations.Tenant;
using API.Data.Models.Tenant;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Contexts;

public class TenantDbContext : DbContext
{
    public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options) { }
    public DbSet<UserCustomFieldValue> UserCustomFieldValues { get; set; }
    public DbSet<TenantUser> Users { get; set; }
    public DbSet<TenantRole> Roles { get; set; }
    public DbSet<TenantUserRole> UserRoles { get; set; }
    public DbSet<TenantPermission> Permissions { get; set; }
    public DbSet<TenantRolePermission> RolePermissions { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfiguration(new TenantUserConfiguration());
        modelBuilder.ApplyConfiguration(new TenantRoleConfiguration());
        modelBuilder.ApplyConfiguration(new TenantUserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new TenantPermissionConfiguration());
        modelBuilder.ApplyConfiguration(new TenantRolePermissionConfiguration());

    }
}

