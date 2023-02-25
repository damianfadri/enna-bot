using Enna.Streamers.Domain.Events;
using Enna.Streamers.Domain.SeedWork;

namespace Enna.Streamers.Domain
{
    public class Feed : Entity
    {
        public FeedType Type { get; init; }
        public DateTime LastNotifiedUtc { get; private set; }

        #region Navigation Properties
#pragma warning disable
        public Streamer Streamer { get; init; }
#pragma warning enable
        #endregion

        public Feed(Guid id, FeedType type) : base(id)
        {
            Type = type;

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
    }
}