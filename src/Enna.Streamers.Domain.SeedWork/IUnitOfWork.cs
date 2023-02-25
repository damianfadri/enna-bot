namespace Enna.Streamers.Domain.SeedWork
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
