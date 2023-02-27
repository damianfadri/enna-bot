namespace Enna.Bot.SeedWork
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
