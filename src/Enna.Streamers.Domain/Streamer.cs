using Enna.Core.Domain;
using Enna.Streamers.Domain.Events;

namespace Enna.Streamers.Domain
{
    public class Streamer : TenantEntity
    {
        public string Name { get; init; }
        public Channel Channel { get; set; }
        public Feed Feed { get; set; }

        public bool IsLive => Channel.IsLive;
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
            Channel.GoLive(streamLink);
            Feed.Notify(Channel);
        }

        public void GoOffline()
        {
            Channel.GoOffline();
        }
    }
}