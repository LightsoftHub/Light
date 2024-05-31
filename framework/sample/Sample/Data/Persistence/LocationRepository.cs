using Light.EntityFrameworkCore.Repositories;

namespace Sample.Data.Persistence;

public interface ILocationRepository : IRepository<RetailLocation>
{
}

public class LocationRepository(
    AlphaDbContext context) :
    Repository<RetailLocation, AlphaDbContext>(context),
    IRepository<RetailLocation>
{
    public override Task<IEnumerable<RetailLocation>> ToListAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Loaded data to cache");
        return base.ToListAsync(cancellationToken);
    }
}
