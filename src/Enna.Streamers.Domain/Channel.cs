using Enna.Core.Domain;

namespace Enna.Streamers.Domain
{
    public class Channel : TenantEntity
    {
        private const int CHANNEL_OFFLINE_THRESHOLD_MINS = 30;

        public string Link { get; init; }
        public string? StreamLink { get; private set; }
        public DateTime StreamStartedUtc { get; private set; }
        public DateTime StreamEndedUtc { get; private set; }
        public DateTime LastFoundOnlineUtc { get; private set; }
        public bool IsLive => StreamLink != null;
        public bool IsOffline => StreamLink == null;

        public Channel(Guid id, string link) : base(id)
        {
            Link = link;
        }

        public void GoLive(string streamLink, DateTime goLiveUtc)
        {
            if (IsOffline)
            {
                StreamLink = streamLink;
                StreamStartedUtc = goLiveUtc;
                StreamEndedUtc = goLiveUtc;
            }

            LastFoundOnlineUtc = goLiveUtc;
        }

        public void GoOffline(DateTime goOfflineUtc)
        {
            if (IsLive && IsOfflineForALongTimeNow(LastFoundOnlineUtc, goOfflineUtc))
            {
                StreamLink = null;
                StreamEndedUtc = goOfflineUtc;
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj is Channel that)
            {
                return object.Equals(Id, that.Id)
                    && object.Equals(Link, that.Link)
                    && object.Equals(StreamLink, that.StreamLink);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode()
                + Link.GetHashCode()
                + (StreamLink?.GetHashCode() ?? 0);
        }

        public static bool IsOfflineForALongTimeNow(
            DateTime lastFoundOnlineUtc, 
            DateTime nowUtc)
        {
            return nowUtc.CompareTo(lastFoundOnlineUtc.AddMinutes(CHANNEL_OFFLINE_THRESHOLD_MINS)) >= 0;
        }

        public static Channel Default => new Channel(Guid.Empty, string.Empty);
    }
}
