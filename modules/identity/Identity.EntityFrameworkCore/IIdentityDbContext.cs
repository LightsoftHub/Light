using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Light.Identity.EntityFrameworkCore;

public interface IIdentityDbContext
{
    DatabaseFacade Database { get; }

    DbSet<UserRole> UserRoles { get; }

    DbSet<UserAttribute> UserAttributes { get; }

    DbSet<JwtToken> JwtTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
