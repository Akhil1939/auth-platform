namespace API.Data.Models.Tenant;

public class TenantRolePermission
{

    public int RoleId { get; set; }
    public TenantRole Role { get; set; }

    public int PermissionId { get; set; }
    public TenantPermission Permission { get; set; }
}
