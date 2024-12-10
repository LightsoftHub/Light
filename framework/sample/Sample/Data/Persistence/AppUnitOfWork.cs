using Light.EntityFrameworkCore.Repositories;

namespace Sample.Data.Persistence
{
    public class AppUnitOfWork(AlphaDbContext context) :
        UnitOfWork(context),
        IAppUnitOfWork
    {
    }
}
