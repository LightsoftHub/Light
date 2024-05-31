using Light.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Light.EntityFrameworkCore.Repositories;

/// <inheritdoc/>
public abstract class UnitOfWorkBase : IUnitOfWork
{
    private readonly DbContext _context;
    private readonly Dictionary<Type, object> _repositories;

    protected UnitOfWorkBase(DbContext context)
    {
        _context = context;
        _repositories = [];
    }

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
            var customRepository = _context.GetService<IRepository<T>>();
            if (customRepository != null)
            {
                _repositories[typeof(T)] = customRepository;
                return customRepository;
            }
        }

        // use default repository
        var repository = new Repository<T>(_context);
        _repositories[typeof(T)] = repository;
        return repository;
    }

    /// <inheritdoc/>
    public virtual int SaveChanges() => _context.SaveChanges();

    /// <inheritdoc/>
    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    /// <inheritdoc/>
    public virtual async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        => await _context.Database.BeginTransactionAsync(cancellationToken);

    /// <inheritdoc/>
    public virtual async Task CommitAsync(CancellationToken cancellationToken = default)
        => await _context.Database.CommitTransactionAsync(cancellationToken);

    /// <inheritdoc/>
    public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
        => await _context.Database.RollbackTransactionAsync(cancellationToken);

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
