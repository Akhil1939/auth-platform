namespace API.Data.Models.Central;

public class MigrationStatus
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public string LastMigration { get; set; }
    public DateTime LastChecked { get; set; } = DateTime.UtcNow;
}
