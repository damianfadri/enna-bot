using Enna.Core.Domain;

namespace Enna.Streamers.Domain.Events
{
    public record StreamerLiveEvent(
        Streamer Streamer,
        Channel Channel)
        : IDomainEvent;
}
