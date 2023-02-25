using Enna.Streamers.Domain;
using FluentAssertions;
using Xunit;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class FindLiveStreamersWorkerTests
    {
        public class Constructor_Should
        {
            [Fact]
            public void ThrowException_When_StreamerRepositoryIsNull()
            {
                var sut = () =>
                    new FindLiveStreamersWorkerSutBuilder()
                        .WithNullStreamerRepository()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ThrowException_When_DomainEventDispatcherIsNull()
            {
                var sut = () =>
                    new FindLiveStreamersWorkerSutBuilder()
                        .WithNullDomainEventDispatcher()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }
        }

        public class DoWork_Should
        {
            [Fact]
            public async Task DoNothing_When_StreamerRepositoryIsEmpty()
            {
                var worker =
                    new FindLiveStreamersWorkerSutBuilder()
                        .WithStreamers()
                        .Build();

                await worker.DoWork();
            }

            [Fact]
            public async Task DoNothing_When_StreamerHasNoChannels()
            {
                var streamer = new Streamer(Guid.NewGuid(), "asd");

                var worker =
                    new FindLiveStreamersWorkerSutBuilder()
                        .WithStreamers(streamer)
                        .Build();

                await worker.DoWork();
            }

            [Fact]
            public async Task DoNothing_When_StreamerHasNoFeeds()
            {
                var channel = new Channel(
                    Guid.NewGuid(), 
                    "https://youtube.com/channel-link");

                channel.GoLive("https://youtube.com/live-link");

                var streamer = new Streamer(Guid.NewGuid(), "asd")
                {
                    Channels = { channel },
                };

                var worker =
                    new FindLiveStreamersWorkerSutBuilder()
                        .WithStreamers(streamer)
                        .WithFetcherDetectingOffline(channel.Link)
                        .Build();

                await worker.DoWork();

                channel.IsLive.Should().BeTrue();
            }

            [Fact]
            public async Task DoNothing_When_StreamerHasNoMatchingFetchers()
            {
                var channel = new Channel(
                    Guid.NewGuid(), 
                    "https://youtube.com/channel-link");

                channel.GoLive("https://youtube.com/live-link");

                var feed = new Feed(Guid.NewGuid(), FeedType.Console);

                var streamer = new Streamer(Guid.NewGuid(), "asd")
                {
                    Channels = { channel },
                    Feeds = { feed },
                };

                var worker =
                    new FindLiveStreamersWorkerSutBuilder()
                        .WithStreamers(streamer)
                        .WithFetcherDetectingOffline("https://twitch.tv/channel-link")
                        .Build();

                await worker.DoWork();

                channel.IsLive.Should().BeTrue();
            }

            [Fact]
            public async Task SetChannelToOffline_When_StreamerIsDetectedAsOffline()
            {
                var channel = new Channel(
                    Guid.NewGuid(), 
                    "https://youtube.com/channel-link");

                channel.GoLive("https://youtube.com/live-link");

                var feed = new Feed(Guid.NewGuid(), FeedType.Console);

                var streamer = new Streamer(Guid.NewGuid(), "asd")
                {
                    Channels = { channel },
                    Feeds = { feed },
                };

                var worker =
                    new FindLiveStreamersWorkerSutBuilder()
                        .WithStreamers(streamer)
                        .WithFetcherDetectingOffline(channel.Link)
                        .Build();

                await worker.DoWork();

                channel.IsOffline.Should().BeTrue();
            }

            [Fact]
            public async Task SetChannelToLive_When_StreamerIsDetectedAsLive()
            {
                var channel = new Channel(
                    Guid.NewGuid(), 
                    "https://youtube.com/channel-link");

                var feed = new Feed(Guid.NewGuid(), FeedType.Console);

                var streamer = new Streamer(Guid.NewGuid(), "asd")
                {
                    Channels = { channel },
                    Feeds = { feed },
                };

                var worker =
                    new FindLiveStreamersWorkerSutBuilder()
                        .WithStreamers(streamer)
                        .WithFetcherDetectingOnline(
                            channel.Link,
                            "https://youtube.com/live-link")
                        .Build();

                await worker.DoWork();

                channel.IsLive.Should().BeTrue();
                channel.StreamLink.Should().Be("https://youtube.com/live-link");
            }
        }
    }
}
