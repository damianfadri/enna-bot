using Enna.Core.Domain;

namespace Enna.Streamers.Domain.Events
{
    public record FeedNotifiedEvent(
        Feed Feed,
        Channel Channel)
        : IDomainEvent;
}