using Enna.Bot.SeedWork;

namespace Enna.Streamers.Domain.Events
{
    public record FeedCreatedEvent(
        Feed Feed)
        : IDomainEvent;
}
