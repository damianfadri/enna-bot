using Enna.Streamers.Domain.SeedWork;

namespace Enna.Streamers.Domain.Events
{
    public record FeedCreatedEvent(
        Feed Feed)
        : IDomainEvent;
}
