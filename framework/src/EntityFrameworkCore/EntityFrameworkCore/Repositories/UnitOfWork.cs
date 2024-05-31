using Light.Repositories;

namespace Light.EntityFrameworkCore.Repositories;

/// <inheritdoc/>
public class UnitOfWork<TContext>(TContext context) :
    UnitOfWorkBase(context),
    IUnitOfWork<TContext>
    where TContext : DbContext
{ }
