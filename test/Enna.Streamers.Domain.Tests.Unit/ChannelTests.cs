using Enna.Streamers.Domain.Events;
using FluentAssertions;
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

                channel.Id.Should().Be(id);
                channel.Link.Should().Be(link);
                channel.StreamLink.Should().BeNull();
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

               
                channel.StreamLink.Should().Be(streamLink);
            }

            [Fact]
            public void UpdateStreamStartedUtc_When_ChannelIsOffline()
            {
                var channel = new Channel(
                    id: Guid.NewGuid(),
                    link: "https://youtube.com/channel-link");

                var oldStreamStartedUtc = channel.StreamStartedUtc;

                channel.GoLive("https://youtube.com/live-link");

                channel.StreamStartedUtc.Should().BeAfter(oldStreamStartedUtc);
            }

            [Fact]
            public void BroadcastStreamerWentLiveEvent_When_ChannelIsOffline()
            {
                var channel = new Channel(
                    id: Guid.NewGuid(),
                    link: "https://youtube.com/channel-link");

                channel.GoLive("https://youtube.com/live-link");

                var @event = channel.GetEvents().Last();

                @event.Should().BeOfType<StreamerLiveEvent>();
                @event.As<StreamerLiveEvent>().Channel.Should().Be(channel);
            }

            [Fact]
            public void SetChannelToLive_When_ChannelIsOffline()
            {
                var channel = new Channel(
                    id: Guid.NewGuid(),
                    link: "https://youtube.com/channel-link");

                channel.GoLive("https://youtube.com/live-link");

                channel.IsLive.Should().BeTrue();
                channel.IsOffline.Should().BeFalse();
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

                channel.StreamStartedUtc.Should().Be(oldStreamStartedUtc);
                channel.GetEvents().Should().BeEmpty();
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

                channel.StreamLink.Should().BeNull();
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

                channel.StreamEndedUtc.Should().BeAfter(oldStreamEndedUtc);
                channel.StreamEndedUtc.Should().BeAfter(channel.StreamStartedUtc);
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

                @event.Should().BeOfType<StreamerOfflineEvent>();
                @event.As<StreamerOfflineEvent>().Channel.Should().Be(channel);
            }

            [Fact]
            public void SetChannelToOffline_When_ChannelIsLive()
            {
                var channel = new Channel(
                    id: Guid.NewGuid(),
                    link: "https://youtube.com/channel-link");

                channel.GoLive("https://youtube.com/live-link");

                channel.GoOffline();

                channel.IsOffline.Should().BeTrue();
                channel.IsLive.Should().BeFalse();
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

                channel.StreamEndedUtc.Should().Be(oldStreamEndedUtc);
                channel.GetEvents().Should().BeEmpty();
            }
        }
    }
}
