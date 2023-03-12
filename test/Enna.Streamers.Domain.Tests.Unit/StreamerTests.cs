using Enna.Streamers.Domain.Events;
using FluentAssertions;
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

                streamer.Id.Should().Be(id);
                streamer.Name.Should().Be(name);
                streamer.Channel.Should().BeNull();
                streamer.Feed.Should().BeNull();
            }

            [Fact]
            public void BroadcastStreamerCreatedEvent()
            {
                var streamer = new Streamer(Guid.NewGuid(), "Friendly name");

                var @event = streamer.GetEvents().Last();

                @event.Should().BeOfType<StreamerCreatedEvent>();
                @event.As<StreamerCreatedEvent>().Streamer.Should().Be(streamer);
            }
        }

        public class GoLive_Should
        {
            [Fact]
            public void SetChannelToLive()
            {
                var channel = new Channel(Guid.NewGuid(), "https://youtube.com/channel-link");

                var streamer = 
                    new Streamer(Guid.NewGuid(), "Friendly name")
                    {
                        Channel = channel
                    };

                streamer.GoLive("https://youtube.com/stream-link");

                streamer.IsLive.Should().BeTrue();
                streamer.StreamLink.Should().Be("https://youtube.com/stream-link");
            }

            [Fact]
            public void NotChangeStreamLink_When_ChannelIsAlreadyLive()
            {
                var channel = new Channel(Guid.NewGuid(), "https://youtube.com/channel-link");
                channel.GoLive("https://youtube.com/stream-link");

                var streamer = 
                    new Streamer(Guid.NewGuid(), "Friendly name")
                    {
                        Channel = channel
                    };

                streamer.GoLive("https://youtube.com/stream-link2");

                streamer.StreamLink.Should().Be("https://youtube.com/stream-link");
            }
        }

        public class GoOffline_Should
        {
            [Fact]
            public void SetChannelToOffline()
            {
                var channel = new Channel(Guid.NewGuid(), "https://youtube.com/channel-link");
                channel.GoLive("https://youtube.com/stream-link");

                var streamer =
                    new Streamer(
                        Guid.NewGuid(), 
                        "Friendly name", 
                        channel, 
                        Feed.Default);

                streamer.GoOffline();

                streamer.IsLive.Should().BeFalse();
                streamer.StreamLink.Should().BeNull();
            }
        }
    }
}
