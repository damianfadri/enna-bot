using Enna.Streamers.Domain.SeedWork;

namespace Enna.Streamers.Domain.Events
{
    public record StreamerOfflineEvent(
        Streamer Streamer,
        Channel Channel)
        : IDomainEvent;
}