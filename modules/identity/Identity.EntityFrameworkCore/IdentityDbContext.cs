using Light.Domain.Entities.Interfaces;
using Light.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Light.Identity.EntityFrameworkCore;

public abstract class IdentityDbContext(DbContextOptions options) :
    IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(options),
    IIdentityDbContext
{
    public virtual DbSet<UserAttribute> UserAttributes => Set<UserAttribute>();

    public virtual DbSet<JwtToken> JwtTokens => Set<JwtToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Role>().ToTable(name: "Roles", Schemas.Identity);

        builder.Entity<RoleClaim>().ToTable(name: "RoleClaims", Schemas.Identity);

        builder.Entity<User>(entity =>
        {
            entity.ToTable(name: "Users", Schemas.Identity);

            // Configure a relationship where the ActiveStatus is owned by (or part of) User.
            entity.OwnsOne(o => o.Status).Property(p => p.Value).HasColumnName("Status");
            entity.Navigation(emp => emp.Status).IsRequired();
        });

        builder.Entity<UserRole>().ToTable(name: "UserRoles", Schemas.Identity);

        builder.Entity<UserLogin>().ToTable(name: "UserLogins", Schemas.Identity);

        builder.Entity<UserClaim>().ToTable(name: "UserClaims", Schemas.Identity);

        builder.Entity<UserToken>().ToTable(name: "UserTokens", Schemas.Identity);

        builder.Entity<UserAttribute>().ToTable(name: "UserAttributes", Schemas.Identity);

        builder.Entity<JwtToken>(e =>
        {
            e.HasKey(x => x.UserId);
            e.ToTable(name: "JwtTokens", Schemas.Identity);
        });
    }

    public override int SaveChanges()
    {
        AuditEntities();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AuditEntities();
        return base.SaveChangesAsync(cancellationToken);
    }

    // for can change action user from inherit class
    protected virtual string CurrentUserId => "anonymous";

    // for can change use soft delete or delete forever from inherit class
    protected virtual bool SoftDelete => false;

    // for can change audit time from inherit class
    protected virtual DateTimeOffset AuditTime => DateTimeOffset.Now;

    protected virtual void AuditEntities()
    {
        // fix null value when delete for Entities inherited ISoftDelete & ValueObjects
        ChangeTracker.Entries<ValueObject>()
            .Where(x => x.State is EntityState.Deleted)
            .ToList()
            .ForEach(e => e.State = EntityState.Unchanged);

        // auto set audit values for Auditable entities
        ChangeTracker.Entries<IAuditableEntity>()
            .ToList()
            .ForEach(e =>
            {
                switch (e.State)
                {
                    case EntityState.Added:
                        e.Entity.CreatedOn = AuditTime;
                        e.Entity.CreatedBy = CurrentUserId;
                        break;

                    case EntityState.Modified:
                        e.Entity.LastModifiedOn = AuditTime;
                        e.Entity.LastModifiedBy = CurrentUserId;
                        break;

                    case EntityState.Deleted:
                        if (e.Entity is ISoftDelete softDelete && SoftDelete)
                        {
                            softDelete.IsDeleted = true;
                            softDelete.DeletedOn = AuditTime;
                            softDelete.DeletedBy = CurrentUserId;
                            e.State = EntityState.Modified;
                        }
                        break;
                }
            });
    }
}
