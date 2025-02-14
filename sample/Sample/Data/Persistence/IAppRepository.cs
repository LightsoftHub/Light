using Light.Domain.Entities.Interfaces;

namespace Sample.Data.Persistence
{
    public interface IAppRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
    }
}
