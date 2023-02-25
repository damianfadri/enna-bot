using Enna.Streamers.Domain.Events;
using Enna.Streamers.Domain.SeedWork;

namespace Enna.Streamers.Domain
{
    public class Streamer : Entity
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