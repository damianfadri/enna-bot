namespace Enna.Streamers.Domain.SeedWork
{
    public interface IDomainEventDispatcher
    {
        Task DispatchEventsAsync();
    }
}
