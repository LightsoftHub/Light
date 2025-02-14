using Light.Domain.Entities.Interfaces;
using Light.Domain.ValueObjects;
using Light.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Sample.Identity;

public class AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : IdentityContext(options)
{
    public override int SaveChanges()
    {
        this.AuditEntries("", DateTime.Now, true);
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.AuditEntries("", DateTime.Now, true);
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

    public void AuditEntries(string? userId, DateTimeOffset auditTime, bool enableSoftDelete = false)
    {
        var changeTracker = ChangeTracker;

        // fix null value when delete for Entities inherited ISoftDelete & ValueObjects
        changeTracker.Entries<ValueObject>()
            .Where(x => x.State is EntityState.Deleted)
            .ToList()
            .ForEach(e => e.State = EntityState.Unchanged);

        // auto set audit values for Auditable entities
        changeTracker.Entries<IAuditableEntity>()
            .ToList()
            .ForEach(e =>
            {
                switch (e.State)
                {
                    case EntityState.Added:
                        e.Entity.CreatedOn = auditTime;
                        e.Entity.CreatedBy = userId;
                        break;

                    case EntityState.Modified:
                        e.Entity.LastModifiedOn = auditTime;
                        e.Entity.LastModifiedBy = userId;
                        break;

                    case EntityState.Deleted:
                        if (e.Entity is ISoftDelete softDelete && enableSoftDelete)
                        {
                            softDelete.IsDeleted = true;
                            softDelete.DeletedOn = auditTime;
                            softDelete.DeletedBy = userId;
                            e.State = EntityState.Modified;
                        }
                        break;
                }
            });
    }
}