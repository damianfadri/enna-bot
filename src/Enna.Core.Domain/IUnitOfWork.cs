namespace Enna.Core.Domain
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
