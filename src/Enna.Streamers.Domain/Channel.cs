using Enna.Streamers.Domain.Events;
using Enna.Streamers.Domain.SeedWork;

namespace Enna.Streamers.Domain
{
    public class Channel : Entity
    {
        public string Link { get; init; }
        public string? StreamLink { get; private set; }
        public DateTime StreamStartedUtc { get; private set; }
        public DateTime StreamEndedUtc { get; private set; }

        public bool IsLive => StreamLink != null;
        public bool IsOffline => StreamLink == null;

        #region Navigation Properties
#pragma warning disable
        public Streamer Streamer { get; init; }
#pragma warning enable
        #endregion

        public Channel(Guid id, string link) : base(id)
        {
            Link = link;
        }

        public void GoLive(string streamLink)
        {
            if (IsOffline)
            {
                StreamLink = streamLink;
                StreamStartedUtc = DateTime.UtcNow;

                AddEvent(new StreamerLiveEvent(Streamer, this));
            }
        }

        public void GoOffline()
        {
            if (IsLive)
            {
                StreamLink = null;
                StreamEndedUtc = DateTime.UtcNow;

                AddEvent(new StreamerOfflineEvent(Streamer, this));
            }
        }
    }
}
