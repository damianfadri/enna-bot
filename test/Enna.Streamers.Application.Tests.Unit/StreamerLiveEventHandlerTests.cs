using Enna.Streamers.Domain;
using Enna.Streamers.Domain.Events;
using FluentAssertions;
using Xunit;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class StreamerLiveEventHandlerTests
    {
        public class Constructor_Should
        {
            [Fact]
            public void ThrowException_When_UnitOfWorkIsNull()
            {
                var sut = () =>
                    new StreamerLiveEventHandlerSutBuilder()
                        .WithNullUnitOfWork()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }
        }

        public class Handle_Should
        {
            [Fact]
            public async Task NotifyFeeds()
            {
                var handler =
                    new StreamerLiveEventHandlerSutBuilder()
                        .Build();

                var feed = new Feed(Guid.NewGuid(), FeedType.Console);
                var streamer = new Streamer(
                    Guid.NewGuid(), 
                    "Streamer name")
                {
                    Feeds = { feed }
                };

                var channel = new Channel(
                    Guid.NewGuid(), 
                    "https://youtube.com/channel-link");

                channel.GoLive("https://youtube.com/stream-link");

                await handler.Handle(
                    new StreamerLiveEvent(
                        streamer, channel),
                    CancellationToken.None);

                feed.GetEvents().Should().NotBeEmpty();

                feed.GetEvents().Last()
                    .Should().BeOfType<FeedNotifiedEvent>();

                feed.GetEvents().Last()
                    .As<FeedNotifiedEvent>().Channel
                    .Should().Be(channel);
            }
        }
    }
}
