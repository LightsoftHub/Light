﻿namespace Light.Identity.EntityFrameworkCore;

public interface IIdentityContext
{
    DbSet<UserRole> UserRoles { get; }

    DbSet<UserAttribute> UserAttributes { get; }

    DbSet<JwtToken> JwtTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
