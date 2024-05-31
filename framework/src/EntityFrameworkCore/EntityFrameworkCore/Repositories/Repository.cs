using Light.Repositories;

namespace Light.EntityFrameworkCore.Repositories;

/// <inheritdoc/>
public class Repository<TEntity>(DbContext context) :
    RepositoryBase<TEntity>(context),
    IRepository<TEntity>
    where TEntity : class
{ }

/// <inheritdoc/>
public class Repository<TEntity, TContext>(TContext context) :
    Repository<TEntity>(context),
    IRepository<TEntity, TContext>
    where TEntity : class
    where TContext : DbContext
{ }