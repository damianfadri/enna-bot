namespace Enna.Bot.SeedWork
{
    public interface IDomainEventDispatcher
    {
        Task DispatchEventsAsync();
    }
}
