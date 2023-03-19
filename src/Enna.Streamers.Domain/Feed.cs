using Enna.Core.Domain;
using Enna.Streamers.Domain.Events;

namespace Enna.Streamers.Domain
{
    public class Feed : TenantEntity
    {
        public FeedType Type { get; init; }
        public string? MessageTemplate { get; init; }
        public DateTime LastNotifiedUtc { get; private set; }

        public Feed(Guid id, FeedType type, string? messageTemplate = null) : base(id)
        {
            Type = type;
            MessageTemplate = messageTemplate;

            AddEvent(new FeedCreatedEvent(this));
        }

        public void Notify(Channel channel, DateTime lastNotifiedUtc)
        {
            if (LastNotifiedUtc < channel.StreamStartedUtc)
            {
                LastNotifiedUtc = lastNotifiedUtc;

                AddEvent(new FeedNotifiedEvent(this, channel));
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj is Feed that)
            {
                return object.Equals(Id, that.Id)
                    && object.Equals(Type, that.Type)
                    && object.Equals(MessageTemplate, that.MessageTemplate)
                    && object.Equals(LastNotifiedUtc, that.LastNotifiedUtc);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode()
                + Type.GetHashCode()
                + (MessageTemplate?.GetHashCode() ?? 0)
                + LastNotifiedUtc.GetHashCode();
        }

        public static Feed Default => new Feed(Guid.Empty, FeedType.Console, null);
    }
}