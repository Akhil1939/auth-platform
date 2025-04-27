using System.ComponentModel.DataAnnotations;

namespace API.Data.Models.Tenant;

public class TenantRole
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public ICollection<TenantUserRole> UserRoles { get; set; }

    public ICollection<TenantRolePermission> RolePermissions { get; set; }
}