namespace API.Data.Models.Tenant;

public class TenantUserRole
{
    public Guid UserId { get; set; }
    public TenantUser User { get; set; }

    public int RoleId { get; set; }
    public TenantRole Role { get; set; }
}
