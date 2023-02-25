using Enna.Streamers.Domain.SeedWork;

namespace Enna.Streamers.Domain.Events
{
    public record StreamerCreatedEvent(
        Streamer Streamer)
        : IDomainEvent;
}