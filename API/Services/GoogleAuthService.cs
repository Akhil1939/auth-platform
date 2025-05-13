using API.Data.Contexts;
using API.Data.Models.Central;
using API.Data.Models.Tenant;
using API.Providers;
using API.Utils;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Services;

public class GoogleAuthService(ITenantDbContextFactory dbFactory, ITenantProvider tenantProvider, JwtHelper jwtHelper)
{
    private readonly ITenantDbContextFactory _dbFactory = dbFactory;
    private readonly ITenantProvider _tenantProvider = tenantProvider;
    private readonly JwtHelper _jwtHelper = jwtHelper;

    public async Task<string> AuthenticateWithGoogleAsync(string idToken, string tenantSlug, CancellationToken cancellationToken)
    {
        GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

        string email = payload.Email;
        string name = payload.Name;
        string picture = payload.Picture;

        Tenant tenant = await _tenantProvider.GetTenantAsync(tenantSlug);
        using TenantDbContext db = _dbFactory.CreateDbContext(tenant.DbConnectionString);

        TenantUser? user = await db.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken: cancellationToken);

        if (user == null)
        {
            // New user, register them
            user = new TenantUser
            {
                Id = Guid.NewGuid(),
                Email = email,
                //Name = name,
                //ProfileImage = picture
            };

            // Optional: assign default role
            TenantRole? defaultRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == "TenantAdmin", cancellationToken: cancellationToken);
            if (defaultRole != null)
            {
                user.UserRoles = [
                    new TenantUserRole { Role = defaultRole }
                ];
            }

            db.Users.Add(user);
            await db.SaveChangesAsync(cancellationToken);
        }

        // Return JWT (replace with your signing logic)
        IEnumerable<Claim> claims = JwtHelper.GetBasicClaims(user.Id.ToString(), user.Email);

        return _jwtHelper.CreateToken(claims);
    }
}
