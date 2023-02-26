namespace Enna.Streamers.Domain.SeedWork
{
    public interface IRepository<T> where T : Entity
    {
        Task Add(T entity);
        Task<IEnumerable<T>> FindAll();
        Task<T?> FindById(Guid id);
        Task Remove(T entity);
    }
}
