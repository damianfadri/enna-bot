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
            public void UpdateLastFoundOnline()
            {
                var channel
                    = new Channel(
                        Guid.NewGuid(),
                        "https://youtube.com/channel-link");

                channel.GoLive(
                    "https://youtube.com/live-link",
                    new DateTime(2021, 11, 12, 13, 53, 10));

                channel.LastFoundOnlineUtc
                    .Should().Be(new DateTime(2021, 11, 12, 13, 53, 10));
            }

            [Fact]
            public void UpdateStreamLink_When_ChannelIsOffline()
            {
                var channel 
                    = new Channel(
                        Guid.NewGuid(),
                        "https://youtube.com/channel-link");

                channel.GoLive(
                    "https://youtube.com/live-link",
                    new DateTime(2011, 06, 23, 09, 38, 27));

                channel.StreamLink.Should().Be("https://youtube.com/live-link");
            }

            [Fact]
            public void UpdateStreamStartedUtc_When_ChannelIsOffline()
            {
                var channel 
                    = new Channel(
                        Guid.NewGuid(),
                        "https://youtube.com/channel-link");

                var oldStreamStartedUtc = channel.StreamStartedUtc;

                channel.GoLive(
                    "https://youtube.com/live-link",
                    new DateTime(1993, 06, 28, 16, 20, 08));

                channel.StreamStartedUtc
                    .Should().BeAfter(oldStreamStartedUtc);
                channel.StreamStartedUtc
                    .Should().Be(new DateTime(1993, 06, 28, 16, 20, 08));
            }

            [Fact]
            public void UpdateStreamEndedUtc_When_ChannelIsOffline()
            {
                var channel
                    = new Channel(
                        Guid.NewGuid(),
                        "https://youtube.com/channel-link");

                var oldStreamStartedUtc = channel.StreamEndedUtc;

                channel.GoLive(
                    "https://youtube.com/live-link",
                    new DateTime(1998, 10, 19, 14, 12, 23));

                channel.StreamEndedUtc
                    .Should().BeAfter(oldStreamStartedUtc);
                channel.StreamEndedUtc
                    .Should().Be(new DateTime(1998, 10, 19, 14, 12, 23));
            }

            [Fact]
            public void SetChannelToLive_When_ChannelIsOffline()
            {
                var channel 
                    = new Channel(
                        Guid.NewGuid(),
                        "https://youtube.com/channel-link");

                channel.GoLive(
                    "https://youtube.com/live-link", 
                    new DateTime(1988, 12, 04, 20, 25, 02));

                channel.IsLive.Should().BeTrue();
                channel.IsOffline.Should().BeFalse();
            }

            [Fact]
            public void DoNothing_When_ChannelIsAlreadyLive()
            {
                var channel 
                    = new Channel(
                        Guid.NewGuid(),
                        "https://youtube.com/channel-link");

                channel.GoLive(
                    "https://youtube.com/live-link", 
                    new DateTime(2017, 05, 06, 18, 43, 29));

                channel.ClearEvents();

                channel.GoLive(
                    "https://youtube.com/live-link2", 
                    new DateTime(2017, 05, 06, 19, 00, 00));

                channel.StreamStartedUtc
                    .Should().Be(new DateTime(2017, 05, 06, 18, 43, 29));
                channel.StreamLink
                    .Should().Be("https://youtube.com/live-link");

                channel.GetEvents().Should().BeEmpty();
            }

            [Fact]
            public void UpdateLastFoundOnlineUtc_When_ChannelIsAlreadyLive()
            {
                var channel
                    = new Channel(
                        Guid.NewGuid(),
                        "https://youtube.com/channel-link");

                channel.GoLive(
                    "https://youtube.com/live-link",
                    new DateTime(2017, 05, 06, 18, 43, 29));

                channel.ClearEvents();

                channel.GoLive(
                    "https://youtube.com/live-link2",
                    new DateTime(2017, 05, 06, 19, 00, 00));

                channel.LastFoundOnlineUtc
                    .Should().Be(new DateTime(2017, 05, 06, 19, 00, 00));

                channel.GetEvents().Should().BeEmpty();
            }
        }

        public class GoOffline_Should
        {
            [Fact]
            public void SetStreamLinkToNull_When_ChannelIsLive()
            {
                var channel 
                    = new Channel(
                        Guid.NewGuid(),
                        "https://youtube.com/channel-link");

                channel.GoLive(
                    "https://youtube.com/live-link", 
                    new DateTime(2021, 10, 02, 09, 30, 30));

                channel.GoOffline(
                    new DateTime(2021, 10, 02, 10, 00, 30));

                channel.StreamLink.Should().BeNull();
            }

            [Fact]
            public void UpdateStreamEndedUtc_When_ChannelIsLive()
            {
                var channel 
                    = new Channel(
                        Guid.NewGuid(),
                        "https://youtube.com/channel-link");

                channel.GoLive(
                    "https://youtube.com/live-link",
                    new DateTime(2021, 10, 10, 22, 15, 00));

                var oldStreamEndedUtc = channel.StreamEndedUtc;

                channel.GoOffline(
                    new DateTime(2021, 10, 10, 22, 45, 00));

                channel.StreamEndedUtc
                    .Should().BeAfter(oldStreamEndedUtc);
                channel.StreamEndedUtc
                    .Should().BeAfter(channel.StreamStartedUtc);
                channel.StreamEndedUtc
                    .Should().Be(new DateTime(2021, 10, 10, 22, 45, 00));
            }

            [Fact]
            public void SetChannelToOffline_When_ChannelIsLive()
            {
                var channel 
                    = new Channel(
                        Guid.NewGuid(),
                        "https://youtube.com/channel-link");

                channel.GoLive(
                    "https://youtube.com/live-link",
                    new DateTime(1979, 12, 12, 06, 15, 00));

                channel.GoOffline(
                    new DateTime(1979, 12, 12, 06, 45, 00));

                channel.IsOffline.Should().BeTrue();
                channel.IsLive.Should().BeFalse();
            }

            [Fact]
            public void DoNothing_When_ChannelIsOffline()
            {
                var channel 
                    = new Channel(
                        Guid.NewGuid(),
                        "https://youtube.com/channel-link");

                channel.GoLive(
                    "https://youtube.com/live-link", 
                    new DateTime(1974, 12, 11, 11, 00, 30));

                channel.GoOffline(
                    new DateTime(1974, 12, 11, 11, 30, 30));

                channel.ClearEvents();

                var oldStreamEndedUtc = channel.StreamEndedUtc;

                channel.GoOffline(
                    new DateTime(1974, 12, 11, 11, 40, 30));

                channel.StreamEndedUtc.Should().Be(oldStreamEndedUtc);
                channel.StreamEndedUtc
                    .Should().Be(new DateTime(1974, 12, 11, 11, 30, 30));

                channel.GetEvents().Should().BeEmpty();
            }

            [Fact]
            public void DoNothing_When_OfflineTimeIsLessThan30Minutes()
            {
                var channel
                    = new Channel(
                        Guid.NewGuid(),
                        "https://youtube.com/channel-link");

                channel.GoLive(
                    "https://youtube.com/live-link",
                    new DateTime(1979, 12, 12, 06, 15, 00));

                channel.GoOffline(
                    new DateTime(1979, 12, 12, 06, 30, 00));

                channel.IsLive.Should().BeTrue();
            }
        }
    }
}
