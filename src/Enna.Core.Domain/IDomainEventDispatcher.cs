namespace Enna.Core.Domain
{
    public interface IDomainEventDispatcher
    {
        Task DispatchEventsAsync();
    }
}
