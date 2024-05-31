using Light.Extensions.Caching;
using Sample.Data.Repository;

namespace Sample.Data.Persistence;

public class CustomCacheRepository : CacheRepositoryBase<AlphaDbContext>
{
    public CustomCacheRepository(AlphaDbContext context, ICacheService cacheService) : base(context, cacheService)
    {
    }

    public override string CacheKey => "CustomDatabase";
}
