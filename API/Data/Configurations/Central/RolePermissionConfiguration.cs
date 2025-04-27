using API.Data.Models.Central;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.Configurations.Central;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

        builder.HasOne(rp => rp.Role)
               .WithMany(r => r.RolePermissions)
               .HasForeignKey(rp => rp.RoleId);

        builder.HasOne(rp => rp.Permission)
               .WithMany(p => p.RolePermissions)
               .HasForeignKey(rp => rp.PermissionId);

        builder.HasData(
            new RolePermission
            {
                RoleId = 1, // SuperAdmin
                PermissionId = 1 // ManageTenants
            },
            new RolePermission
            {
                RoleId = 1,
                PermissionId = 2 // ViewLogs
            }
        );
    }
}