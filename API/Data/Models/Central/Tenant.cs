namespace API.Data.Models.Central;

public class Tenant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Slug { get; set; } // used in URLs
    public string DbConnectionString { get; set; }
    public string BrandingJson { get; set; } // e.g., JSON config for logo/colors
    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
    //TODO created by
}
