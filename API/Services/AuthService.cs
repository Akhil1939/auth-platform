using API.Data.Contexts;
using API.Data.Models.Central;
using API.Providers;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace API.Services;

public class AuthService
{
    private readonly ITenantDbContextFactory _dbFactory;
    private readonly ITenantProvider _tenantProvider;

    public AuthService(ITenantDbContextFactory dbFactory, ITenantProvider tenantProvider)
    {
        _dbFactory = dbFactory;
        _tenantProvider = tenantProvider;
    }

    public async Task<bool> RegisterUserAsync(string tenantSlug, string email, string password)
    {
        Data.Models.Tenant.Tenant tenant = await _tenantProvider.GetTenantAsync(tenantSlug);
        using TenantDbContext db = _dbFactory.CreateDbContext(tenant.DbConnectionString);

        if (await db.Users.AnyAsync(u => u.Email == email))
            throw new Exception("User already exists");

        User user = new()
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<string> LoginAsync(string tenantSlug, string email, string password)
    {
        Data.Models.Tenant.Tenant tenant = await _tenantProvider.GetTenantAsync(tenantSlug);
        using TenantDbContext db = _dbFactory.CreateDbContext(tenant.DbConnectionString);

        User? user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new Exception("Invalid credentials");

        // Return JWT (replace with your signing logic)
        return GenerateFakeJwt(user.Email);
    }

    private string GenerateFakeJwt(string email)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes($"jwt-for:{email}"));
    }
}