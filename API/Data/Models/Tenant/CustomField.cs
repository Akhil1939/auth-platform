namespace API.Data.Models.Tenant;

public class CustomField
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public string FieldName { get; set; }
    public string FieldType { get; set; } // text, dropdown, etc.
    public bool IsRequired { get; set; }
    public string[]? Options { get; set; } // if dropdown
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
