using Enna.Streamers.Domain;
using Enna.Streamers.Domain.Events;
using Xunit;

namespace Enna.Streamers.Domain.Tests.Unit
{
    public class ChannelTests
    {
        public class Constructor_Should
        {
            [Fact]
            public void SetProperties()
            {
                var id = Guid.NewGuid();
                var link = "https://youtube.com/channel-link";

                var channel = new Channel(id, link);

                Assert.Equal(id, channel.Id);
                Assert.Equal(link, channel.Link);
                Assert.Null(channel.StreamLink);
            }
        }

        public class GoLive_Should
        {
            [Fact]
            public void UpdateStreamLink_When_ChannelIsOffline()
            {
                var channel = new Channel(
                    id: Guid.NewGuid(),
                    link: "https://youtube.com/channel-link");

                var streamLink = "https://youtube.com/live-link";
                channel.GoLive(streamLink);

                Assert.Equal(streamLink, channel.StreamLink);
            }

            [Fact]
            public void UpdateStreamStartedUtc_When_ChannelIsOffline()
            {
                var channel = new Channel(
                    id: Guid.NewGuid(),
                    link: "https://youtube.com/channel-link");

                var oldStreamStartedUtc = channel.StreamStartedUtc;

                channel.GoLive("https://youtube.com/live-link");

                Assert.True(channel.StreamStartedUtc > oldStreamStartedUtc);
            }

            [Fact]
            public void BroadcastStreamerWentLiveEvent_When_ChannelIsOffline()
            {
                var channel = new Channel(
                    id: Guid.NewGuid(),
                    link: "https://youtube.com/channel-link");

                channel.GoLive("https://youtube.com/live-link");

                var @event = channel.GetEvents().Last();

                Assert.IsType<StreamerLiveEvent>(@event);

                Assert.Equal(channel.Id,
                    ((StreamerLiveEvent)@event).Channel.Id);
            }

            [Fact]
            public void SetChannelToLive_When_ChannelIsOffline()
            {
                var channel = new Channel(
                    id: Guid.NewGuid(),
                    link: "https://youtube.com/channel-link");

                channel.GoLive("https://youtube.com/live-link");

                Assert.True(channel.IsLive);
                Assert.False(channel.IsOffline);
            }

            [Fact]
            public void DoNothing_When_ChannelIsLive()
            {
                var channel = new Channel(
                    id: Guid.NewGuid(),
                    link: "https://youtube.com/channel-link");

                channel.GoLive("https://youtube.com/live-link");
                channel.ClearEvents();

                var oldStreamStartedUtc = channel.StreamStartedUtc;

                channel.GoLive("https://youtube.com/live-link2");

                Assert.Equal(oldStreamStartedUtc, channel.StreamStartedUtc);
                Assert.Empty(channel.GetEvents());
            }
        }

        public class GoOffline_Should
        {
            [Fact]
            public void SetStreamLinkToNull_When_ChannelIsLive()
            {
                var channel = new Channel(
                    id: Guid.NewGuid(),
                    link: "https://youtube.com/channel-link");

                var streamLink = "https://youtube.com/live-link";
                channel.GoLive(streamLink);

                channel.GoOffline();

                Assert.Null(channel.StreamLink);
            }

            [Fact]
            public void UpdateStreamEndedUtc_When_ChannelIsLive()
            {
                var channel = new Channel(
                    id: Guid.NewGuid(),
                    link: "https://youtube.com/channel-link");

                channel.GoLive("https://youtube.com/live-link");

                var oldStreamEndedUtc = channel.StreamEndedUtc;

                channel.GoOffline();

                Assert.True(channel.StreamEndedUtc > oldStreamEndedUtc);
            }

            [Fact]
            public void BroadcastStreamerWentOfflineEvent_When_ChannelIsLive()
            {
                var channel = new Channel(
                    id: Guid.NewGuid(),
                    link: "https://youtube.com/channel-link");

                channel.GoLive("https://youtube.com/live-link");

                channel.GoOffline();

                var @event = channel.GetEvents().Last();

                Assert.IsType<StreamerOfflineEvent>(@event);

                Assert.Equal(channel.Id,
                    ((StreamerOfflineEvent)@event).Channel.Id);
            }

            [Fact]
            public void SetChannelToOffline_When_ChannelIsLive()
            {
                var channel = new Channel(
                    id: Guid.NewGuid(),
                    link: "https://youtube.com/channel-link");

                channel.GoLive("https://youtube.com/live-link");

                channel.GoOffline();

                Assert.True(channel.IsOffline);
                Assert.False(channel.IsLive);
            }

            [Fact]
            public void DoNothing_When_ChannelIsOffline()
            {
                var channel = new Channel(
                    id: Guid.NewGuid(),
                    link: "https://youtube.com/channel-link");

                channel.GoLive("https://youtube.com/live-link");
                channel.GoOffline();

                channel.ClearEvents();

                var oldStreamEndedUtc = channel.StreamEndedUtc;

                channel.GoOffline();

                Assert.Equal(oldStreamEndedUtc, channel.StreamEndedUtc);
                Assert.Empty(channel.GetEvents());
            }
        }
    }
}
