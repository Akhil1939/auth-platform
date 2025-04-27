namespace API.Data.Models.Tenant;

public class TenantPermission
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<TenantRolePermission> RolePermissions { get; set; }
}
