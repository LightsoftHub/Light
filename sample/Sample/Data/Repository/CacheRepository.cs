using Light.Extensions.Caching;
using Microsoft.EntityFrameworkCore;

namespace Sample.Data.Repository;

public class CacheRepository<TEntity, TContext>(TContext context, ICacheService cacheService)
    : CacheRepositoryBase<TEntity>(context, cacheService), ICacheRepository<TEntity, TContext>
    where TEntity : class
    where TContext : DbContext
{
}