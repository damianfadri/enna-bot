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
            public void SetDefaultProperties()
            {
                var id = Guid.NewGuid();
                var name = "Friendly name";

                var streamer = new Streamer(id, name);

                streamer.Id.Should().Be(id);
                streamer.Name.Should().Be(name);
                streamer.Channel.Should().Be(Channel.Default);
                streamer.Feed.Should().Be(Feed.Default);
            }

            [Fact]
            public void SetProperties()
            {
                var id = Guid.NewGuid();
                var name = "Friendly name";

                var channel 
                    = new Channel(
                        Guid.NewGuid(), 
                        "https://youtube.com/channel-link");

                var feed 
                    = new Feed(
                        Guid.NewGuid(), 
                        FeedType.Discord);

                var streamer = new Streamer(id, name, channel, feed);

                streamer.Id.Should().Be(id);
                streamer.Name.Should().Be(name);
                streamer.Channel.Should().Be(channel);
                streamer.Feed.Should().Be(feed);
            }

            [Fact]
            public void BroadcastStreamerCreatedEvent()
            {
                var streamer = new Streamer(Guid.NewGuid(), "Friendly name");

                var @event = streamer.GetEvents().Last();

                @event.Should().BeOfType<StreamerCreatedEvent>();
                @event.As<StreamerCreatedEvent>().Streamer
                    .Should().Be(streamer);
            }
        }

        public class GoLive_Should
        {
            [Fact]
            public void SetChannelToLive()
            {
                var streamer =
                    new Streamer(
                        Guid.NewGuid(), 
                        "Friendly name");

                streamer.GoLive(
                    "https://youtube.com/stream-link",
                    new DateTime(2020, 02, 22, 14, 34, 19));

                streamer.IsLive.Should().BeTrue();
                streamer.StreamLink
                    .Should().Be("https://youtube.com/stream-link");
            }

            [Fact]
            public void NotChangeStreamLink_When_ChannelIsAlreadyLive()
            {
                var channel 
                    = new Channel(
                        Guid.NewGuid(), 
                        "https://youtube.com/channel-link");

                channel.GoLive(
                    "https://youtube.com/stream-link",
                    new DateTime(1983, 09, 02, 15, 20, 00));

                var streamer =
                    new Streamer(
                        Guid.NewGuid(), 
                        "Friendly name", 
                        channel, 
                        Feed.Default);

                streamer.GoLive(
                    "https://youtube.com/stream-link2",
                    new DateTime(1983, 09, 03, 15, 20, 00));

                streamer.IsLive.Should().BeTrue();
                streamer.StreamLink
                    .Should().Be("https://youtube.com/stream-link");
            }

            [Fact]
            public void BroadcastStreamerLiveEvent()
            {
                var streamer =
                    new Streamer(
                        Guid.NewGuid(),
                        "Friendly name");

                streamer.GoLive(
                    "https://youtube.com/live-link", 
                    new DateTime(2004, 12, 03, 20, 20, 45));

                var @event = streamer.GetEvents().Last();

                @event.Should().BeOfType<StreamerLiveEvent>();
                @event.As<StreamerLiveEvent>().Streamer.Should().Be(streamer);
                @event.As<StreamerLiveEvent>().Channel.Should().Be(streamer.Channel);
            }
        }

        public class GoOffline_Should
        {
            [Fact]
            public void SetChannelToOffline()
            {
                var channel 
                    = new Channel(
                        Guid.NewGuid(), 
                        "https://youtube.com/channel-link");

                channel.GoLive(
                    "https://youtube.com/stream-link",
                    new DateTime(2002, 03, 07, 16, 10, 43));

                var streamer 
                    = new Streamer(
                        Guid.NewGuid(), 
                        "Friendly name", 
                        channel, 
                        Feed.Default);

                streamer.GoOffline(
                    new DateTime(2002, 03, 07, 16, 40, 43));

                streamer.IsLive.Should().BeFalse();
                streamer.StreamLink.Should().BeNull();
            }

            [Fact]
            public void BroadcastStreamerOfflineEvent()
            {
                var channel
                    = new Channel(
                        Guid.NewGuid(),
                        "https://youtube.com/channel-link");

                channel.GoLive(
                    "https://youtube.com/stream-link", 
                    new DateTime(1991, 10, 17, 13, 04, 23));

                var streamer =
                    new Streamer(
                        Guid.NewGuid(),
                        "Friendly name",
                        channel,
                        Feed.Default);

                streamer.GoOffline(
                    new DateTime(1991, 10, 17, 13, 34, 23));

                var @event = streamer.GetEvents().Last();

                @event.Should().BeOfType<StreamerOfflineEvent>();
                @event.As<StreamerOfflineEvent>().Streamer.Should().Be(streamer);
                @event.As<StreamerOfflineEvent>().Channel.Should().Be(streamer.Channel);
            }
        }
    }
}
