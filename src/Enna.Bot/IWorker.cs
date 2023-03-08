namespace Enna.Bot
{
    public interface IWorker
    {
        Task DoWork(params object[] args);
    }
}
