using Light.Domain.Entities.Interfaces;
using Light.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests;

public class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
{
    public virtual DbSet<Product> Products => Set<Product>();

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
                        e.Entity.Created = TestValues.CreateAtTime;
                        e.Entity.CreatedBy = TestValues.CreateByUserId;
                        break;

                    case EntityState.Modified:
                        e.Entity.LastModified = TestValues.UpdateAtTime;
                        e.Entity.LastModifiedBy = TestValues.UpdateByUserId;
                        break;

                    case EntityState.Deleted:
                        if (e.Entity is ISoftDelete softDelete)
                        {
                            softDelete.IsDeleted = true;
                            softDelete.DeletedOn = TestValues.DeleteAtTime;
                            softDelete.DeletedBy = TestValues.DeleteByUserId;
                            e.State = EntityState.Modified;
                        }
                        break;
                }
            });

        ChangeTracker.Entries<ITenant>()
            .ToList()
            .ForEach(e =>
            {
                switch (e.State)
                {
                    case EntityState.Added:
                        e.Entity.TenantId = TestValues.TenandId;
                        break;
                }
            });
    }
}
