using Enna.Core.Domain;
using Enna.Streamers.Domain.Events;

namespace Enna.Streamers.Domain
{
    public class Streamer : TenantEntity
    {
        public string Name { get; init; }
        public List<Channel> Channels { get; set; }
        public List<Feed> Feeds { get; set; }

        public Streamer(Guid id, string name) : base(id)
        {
            Name = name;
            Channels = new();
            Feeds = new();

            AddEvent(new StreamerCreatedEvent(this));
        }
    }
}