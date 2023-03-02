using Enna.Core.Domain;

namespace Enna.Streamers.Domain.Events
{
    public record FeedCreatedEvent(
        Feed Feed)
        : IDomainEvent;
}
