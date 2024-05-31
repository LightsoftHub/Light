using Microsoft.EntityFrameworkCore;

namespace Sample.Data.Repository
{
    /// <summary>
    ///     Can be used to auto load list of T on cache,
    ///         query, add, update, remove instances of T.
    /// </summary>
    public interface ICacheRepository<T> : IRepository<T> where T : class
    {
        /// <summary>
        ///     Delete list of T on cache.
        /// </summary>
        Task RemoveCacheAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Reload list of T on cache.
        /// </summary>
        Task ReloadCacheAsync(CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Cache repository with multi DbContext
    /// </summary>
    public interface ICacheRepository<TEntity, TContext> : ICacheRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
    }
}