using System.ComponentModel.DataAnnotations;

namespace API.Data.Models.Tenant;

public class TenantUser
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Salt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<TenantUserRole> UserRoles { get; set; }
}
