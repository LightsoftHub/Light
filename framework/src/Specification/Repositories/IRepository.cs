namespace Light.Repositories
{
    /// <summary>
    ///     Can be used to query, add, update, remove instances of T
    /// </summary>
    public interface IRepository<T> : IRepositoryBase<T>, ISaveChanges where T : class
    {
    }
}