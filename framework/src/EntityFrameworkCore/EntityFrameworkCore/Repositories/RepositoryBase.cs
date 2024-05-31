using Light.Repositories;
using Light.Specification;
using System.Linq.Expressions;

namespace Light.EntityFrameworkCore.Repositories;

/// <inheritdoc/>
public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
    where TEntity : class
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    protected RepositoryBase(DbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    /// <inheritdoc/>
    public virtual IQueryable<TEntity> Include<TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath)
        => _context.Set<TEntity>().Include(navigationPropertyPath);

    /// <inheritdoc/>
    public virtual IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        => _context.Set<TEntity>().Where(expression);

    /// <inheritdoc/>
    public virtual IQueryable<TEntity> WhereIf(bool condition, Expression<Func<TEntity, bool>> expression)
        => _context.Set<TEntity>().WhereIf(condition, expression);

    /// <inheritdoc/>
    public virtual IQueryable<TEntity> Where(ISpecification<TEntity> specification)
        => _context.Set<TEntity>().Where(specification);

    /// <inheritdoc/>
    public virtual IQueryable<TEntity> WhereIf(bool condition, ISpecification<TEntity> specification)
        => _context.Set<TEntity>().WhereIf(condition, specification);

    /// <inheritdoc/>
    public virtual void Add(TEntity entity) => _dbSet.Add(entity);

    /// <inheritdoc/>
    public virtual void AddRange(IEnumerable<TEntity> entities) => _dbSet.AddRange(entities);

    /// <inheritdoc/>
    public virtual void Update(TEntity entity) => _dbSet.Update(entity);

    /// <inheritdoc/>
    public virtual void UpdateRange(IEnumerable<TEntity> entities) => _dbSet.UpdateRange(entities);

    /// <inheritdoc/>
    public virtual void Remove(TEntity entity) => _dbSet.Remove(entity);

    /// <inheritdoc/>
    public virtual void RemoveRange(IEnumerable<TEntity> entities) => _dbSet.RemoveRange(entities);

    /// <inheritdoc/>
    public virtual int SaveChanges() => _context.SaveChanges();

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<TEntity>> ToListAsync(CancellationToken cancellationToken = default)
        => await _dbSet.ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public virtual async Task<TEntity?> FindByKeyAsync<TKey>(TKey key, CancellationToken cancellationToken = default) where TKey : notnull
        => await _context.FindAsync<TEntity>(key, cancellationToken);

    /// <inheritdoc/>
    public virtual async Task<TEntity?> FindByKeyAsync(object?[] key, CancellationToken cancellationToken = default)
        => await _context.FindAsync<TEntity>(key, cancellationToken);

    /// <inheritdoc/>
    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await _dbSet.AddAsync(entity, cancellationToken);

    /// <inheritdoc/>
    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        => await _dbSet.AddRangeAsync(entities, cancellationToken);

    /// <inheritdoc/>
    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}