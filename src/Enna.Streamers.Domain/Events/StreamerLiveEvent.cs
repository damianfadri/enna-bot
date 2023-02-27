using Enna.Bot.SeedWork;

namespace Enna.Streamers.Domain.Events
{
    public record StreamerLiveEvent(
        Streamer Streamer,
        Channel Channel)
        : IDomainEvent;
}
