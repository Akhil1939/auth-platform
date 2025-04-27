using API.Data.Models.Central;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.Configurations.Central;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => p.Name).IsUnique();
        builder.Property(p => p.Name).IsRequired();

        builder.HasData(
            new Permission { Id = 1, Name = "ManageTenants" },
            new Permission { Id = 2, Name = "ViewLogs" }
        );
    }
}