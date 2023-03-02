using Enna.Core.Domain;

namespace Enna.Streamers.Domain.Events
{
    public record StreamerCreatedEvent(
        Streamer Streamer)
        : IDomainEvent;
}