using Light.EntityFrameworkCore.Repositories;
using Light.Extensions.Caching;
using Microsoft.EntityFrameworkCore;

namespace Sample.Data.Repository;

public abstract class CacheRepositoryBase<T> : Repository<T>, ICacheRepository<T>
    where T : class
{
    private readonly ICacheService _cacheService;
    private readonly DbContext _context;

    protected CacheRepositoryBase(DbContext context, ICacheService cacheService) : base(context)
    {
        _context = context;
        _cacheService = cacheService;
    }

    public virtual string CacheKey
    {
        get
        {
            var databaseName = _context.Database.GetDbConnection().Database;
            return $"[{databaseName}]_[{typeof(T).Name}]";
        }
    }

    private async Task<IEnumerable<T>> LoadDataToCacheAsync(CancellationToken cancellationToken = default)
    {
        var key = CacheKey;

        await _cacheService.RemoveAsync(key, cancellationToken);

        var data = await base.ToListAsync(cancellationToken);

        var lifeTime = TimeSpan.FromMinutes(30);

        await _cacheService.TrySetAsync(key, data, lifeTime, cancellationToken: cancellationToken);

        return data;
    }

    public override async Task<IEnumerable<T>> ToListAsync(CancellationToken cancellationToken = default)
    {
        var key = CacheKey;

        var data = await _cacheService.TryGetAsync<IEnumerable<T>>(key, cancellationToken);

        data ??= await LoadDataToCacheAsync(cancellationToken);

        return data;
    }

    public override int SaveChanges()
    {
        var result = base.SaveChanges();
        Task.Run(async () => await ReloadCacheAsync());
        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        await ReloadCacheAsync(cancellationToken: cancellationToken);
        return result;
    }

    public async Task ReloadCacheAsync(CancellationToken cancellationToken = default)
    {
        await LoadDataToCacheAsync(cancellationToken);
    }

    public virtual async Task RemoveCacheAsync(CancellationToken cancellationToken = default)
    {
        var key = CacheKey;
        await _cacheService.RemoveAsync(key, cancellationToken);
    }
}
