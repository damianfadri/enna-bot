using Enna.Core.Domain;
using Enna.Streamers.Domain.Events;

namespace Enna.Streamers.Domain
{
    public class Feed : TenantEntity
    {
        public FeedType Type { get; init; }
        public string? MessageTemplate { get; init; }
        public DateTime LastNotifiedUtc { get; private set; }

        #region Navigation Properties
#pragma warning disable
        public Streamer Streamer { get; init; }
#pragma warning enable
        #endregion

        public Feed(Guid id, FeedType type, string? messageTemplate) : base(id)
        {
            Type = type;
            MessageTemplate = messageTemplate;

            AddEvent(new FeedCreatedEvent(this));
        }

        public void Notify(Channel channel)
        {
            if (LastNotifiedUtc < channel.StreamStartedUtc)
            {
                LastNotifiedUtc = DateTime.UtcNow;

                AddEvent(new FeedNotifiedEvent(this, channel));
            }
        }

        public static Feed Default => new Feed(Guid.NewGuid(), FeedType.Console, null);
    }
}