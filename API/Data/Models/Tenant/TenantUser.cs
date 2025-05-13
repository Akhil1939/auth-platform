using System.ComponentModel.DataAnnotations;

namespace API.Data.Models.Tenant;

public class TenantUser
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Email { get; set; }

    public string Password { get; set; }

    public string Salt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<TenantUserRole> UserRoles { get; set; }
}
