using Enna.Core.Domain;

namespace Enna.Streamers.Domain.Events
{
    public record StreamerOfflineEvent(
        Streamer Streamer,
        Channel Channel)
        : IDomainEvent;
}