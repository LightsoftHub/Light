namespace Light.Identity.EntityFrameworkCore;

public interface IIdentityDbContext
{
    DbSet<UserRole> UserRoles { get; }

    DbSet<UserAttribute> UserAttributes { get; }

    DbSet<JwtToken> JwtTokens { get; }

    DbSet<Tenant> Tenants { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
