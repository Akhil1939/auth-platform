using API.Data.Contexts;
using API.Data.Models.Tenant;
using Microsoft.EntityFrameworkCore;

namespace API.Providers;

public interface ITenantProvider
{
    Task<Tenant> GetTenantAsync(string slug);
}
public class TenantProvider : ITenantProvider
{
    private readonly CentralDbContext _db;

    public TenantProvider(CentralDbContext db)
    {
        _db = db;
    }

    public async Task<Tenant> GetTenantAsync(string slug)
    {
        return await _db.Tenants.FirstOrDefaultAsync(t => t.Slug == slug) ?? throw new Exception();
    }
}
