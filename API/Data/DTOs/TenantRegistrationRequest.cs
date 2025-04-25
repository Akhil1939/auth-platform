namespace API.Data.DTOs;

public class TenantRegistrationRequest
{
    public string Name { get; set; }
    public string Slug { get; set; } // unique company identifier
    public string AdminEmail { get; set; }
    public string AdminPassword { get; set; }
}