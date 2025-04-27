namespace API.Data.Models.Central;

public class RolePermission
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int RoleId { get; set; }
    public Role Role { get; set; }

    public int PermissionId { get; set; }
    public Permission Permission { get; set; }
}
