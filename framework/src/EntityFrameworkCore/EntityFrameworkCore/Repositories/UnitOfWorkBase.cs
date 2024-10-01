using Light.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Light.EntityFrameworkCore.Repositories;

/// <inheritdoc/>
public abstract class UnitOfWorkBase(DbContext context) : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = [];

    /// <inheritdoc/>
    public IRepositoryBase<T> Repository<T>(bool useCustomRepository = false)
        where T : class
    {
        _repositories.TryGetValue(typeof(T), out var value);
        if (value != null)
        {
            return (IRepositoryBase<T>)value;
        }

        if (useCustomRepository)
        {
            // use custom repository if available
            var customRepository = context.GetService<IRepository<T>>();
            if (customRepository != null)
            {
                _repositories[typeof(T)] = customRepository;
                return customRepository;
            }
        }

        // use default repository
        var repository = new Repository<T>(context);
        _repositories[typeof(T)] = repository;
        return repository;
    }

    /// <inheritdoc/>
    public virtual int SaveChanges() => context.SaveChanges();

    /// <inheritdoc/>
    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);

    /// <inheritdoc/>
    public virtual async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        => await context.Database.BeginTransactionAsync(cancellationToken);

    /// <inheritdoc/>
    public virtual async Task CommitAsync(CancellationToken cancellationToken = default)
        => await context.Database.CommitTransactionAsync(cancellationToken);

    /// <inheritdoc/>
    public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
        => await context.Database.RollbackTransactionAsync(cancellationToken);

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
