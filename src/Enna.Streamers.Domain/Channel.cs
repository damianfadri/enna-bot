using Enna.Core.Domain;
using Enna.Streamers.Domain.Events;

namespace Enna.Streamers.Domain
{
    public class Channel : TenantEntity
    {
        public string Link { get; init; }
        public string? StreamLink { get; private set; }
        public DateTime StreamStartedUtc { get; private set; }
        public DateTime StreamEndedUtc { get; private set; }

        public bool IsLive => StreamLink != null;
        public bool IsOffline => StreamLink == null;

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
            }
        }

        public void GoOffline()
        {
            if (IsLive)
            {
                StreamLink = null;
                StreamEndedUtc = DateTime.UtcNow;
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

        public static Channel Default => new Channel(Guid.Empty, string.Empty);
    }
}
