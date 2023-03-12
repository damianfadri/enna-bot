using Enna.Core.Domain;
using Enna.Streamers.Domain.Events;

namespace Enna.Streamers.Domain
{
    public class Streamer : TenantEntity
    {
        public string Name { get; init; }
        public Channel Channel { get; init; }
        public Feed Feed { get; init; }

        public bool IsLive => Channel.IsLive;
        public bool IsOffline => !IsLive;
        public string? StreamLink => Channel.StreamLink;

        public Streamer(Guid id, string name) : this(id, name, Channel.Default, Feed.Default)
        {
        }

        public Streamer(Guid id, string name, Channel channel, Feed feed) : base(id)
        {
            Name = name;
            Channel = channel;
            Feed = feed;

            AddEvent(new StreamerCreatedEvent(this));
        }

        public void GoLive(string streamLink)
        {
            if (IsOffline)
            {
                Channel.GoLive(streamLink);
                Feed.Notify(Channel);

                AddEvent(new StreamerLiveEvent(this, Channel));
            }
        }

        public void GoOffline()
        {
            if (IsLive)
            {
                Channel.GoOffline();

                AddEvent(new StreamerOfflineEvent(this, Channel));
            }
        }
    }
}