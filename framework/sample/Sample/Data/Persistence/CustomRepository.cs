using Light.Domain.Entities.Interfaces;
using Light.EntityFrameworkCore.Repositories;

namespace Sample.Data.Persistence;

public class CustomRepository<TEntity>(
    AlphaDbContext context) :
    RepositoryBase<TEntity>(context),
    IAppRepository<TEntity>
    where TEntity : class, IEntity
{
    public override Task<IEnumerable<TEntity>> ToListAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Use from Custom repository");
        return base.ToListAsync(cancellationToken);
    }
}
