using API.Data.Models.Central;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Contexts;

public class TenantDbContext : DbContext
{
    public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<UserCustomFieldValue> UserCustomFieldValues { get; set; }
}

