using Enna.Streamers.Domain;
using Enna.Streamers.Domain.Events;
using Xunit;

namespace Enna.Streamers.Domain.Tests.Unit
{
    public class StreamerTests
    {
        public class Constructor_Should
        {
            [Fact]
            public void SetProperties()
            {
                var id = Guid.NewGuid();
                var name = "Friendly name";

                var streamer = new Streamer(id, name);

                Assert.Equal(id, streamer.Id);
                Assert.Equal(name, streamer.Name);
                Assert.Empty(streamer.Channels);
                Assert.Empty(streamer.Feeds);
            }

            [Fact]
            public void BroadcastStreamerCreatedEvent()
            {
                var streamer = new Streamer(Guid.NewGuid(), "Friendly name");

                var @event = streamer.GetEvents().Last();

                Assert.IsType<StreamerCreatedEvent>(@event);
                Assert.Equal(streamer.Id, ((StreamerCreatedEvent)@event).Streamer.Id);
            }
        }
    }
}
