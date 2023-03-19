using Enna.Streamers.Domain.Events;
using FluentAssertions;
using Xunit;

namespace Enna.Streamers.Domain.Tests.Unit
{
    public class FeedTests
    {
        public class Constructor_Should
        {
            [Fact]
            public void SetDefaultProperties()
            {
                var feed = Feed.Default;

                feed.Id.Should().Be(Guid.Empty);
                feed.Type.Should().Be(FeedType.Console);
                feed.MessageTemplate.Should().BeNull();
            }

            [Fact]
            public void SetProperties()
            {
                var id = Guid.NewGuid();

                var feed = new Feed(id, FeedType.Discord, "@link");

                feed.Id.Should().Be(id);
                feed.Type.Should().Be(FeedType.Discord);
                feed.MessageTemplate.Should().Be("@link");
            }

            [Fact]
            public void BroadcastFeedCreatedEvent()
            {
                var feed = new Feed(Guid.NewGuid(), FeedType.Console, "@link");

                var @event = feed.GetEvents().Last();

                @event.Should().BeOfType<FeedCreatedEvent>();
                @event.As<FeedCreatedEvent>().Feed.Should().Be(feed);
            }
        }

        public class Notify_Should
        {
            [Fact]
            public void UpdateLastNotifiedUtc_When_FeedIsNotYetNotified()
            {
                var channel 
                    = new Channel(
                        Guid.NewGuid(), 
                        "https://youtube.com/some-channel");

                channel.GoLive(
                    "https://youtube.com/live-link", 
                    new DateTime(1993, 08, 23, 03, 49, 52));

                var feed = new Feed(Guid.NewGuid(), FeedType.Console, "@link");
                var oldLastNotifiedUtc = feed.LastNotifiedUtc;

                feed.Notify(
                    channel, 
                    new DateTime(1993, 08, 23, 03, 49, 52));

                feed.LastNotifiedUtc.Should().BeAfter(oldLastNotifiedUtc);
                feed.LastNotifiedUtc
                    .Should().Be(new DateTime(1993, 08, 23, 03, 49, 52));
            }

            [Fact]
            public void BroadcastFeedNotifiedEvent_When_FeedIsNotYetNotified()
            {
                var channel 
                    = new Channel(
                        Guid.NewGuid(), 
                        "https://youtube.com/some-channel");

                channel.GoLive(
                    "https://youtube.com/live-link", 
                    new DateTime(2005, 12, 07, 21, 53, 03));

                var feed = new Feed(Guid.NewGuid(), FeedType.Console, "@link");

                feed.Notify(
                    channel, 
                    new DateTime(2005, 12, 07, 21, 53, 03));

                var @event = feed.GetEvents().Last();

                @event.Should().BeOfType<FeedNotifiedEvent>();
                @event.As<FeedNotifiedEvent>().Feed.Should().Be(feed);
                @event.As<FeedNotifiedEvent>().Channel.Should().Be(channel);
            }

            [Fact]
            public void DoNothing_When_FeedIsAlreadyNotified()
            {
                var channel 
                    = new Channel(
                        Guid.NewGuid(), 
                        "https://youtube.com/some-channel");

                channel.GoLive(
                    "https://youtube.com/live-link", 
                    new DateTime(1976, 03, 16, 07, 20, 55));

                var feed = new Feed(Guid.NewGuid(), FeedType.Console, "@link");

                feed.Notify(
                    channel, 
                    new DateTime(1976, 03, 16, 07, 20, 55));

                feed.ClearEvents();

                feed.Notify(
                    channel,
                    new DateTime(1976, 03, 16, 08, 30, 15));

                feed.LastNotifiedUtc
                    .Should().Be(new DateTime(1976, 03, 16, 07, 20, 55));

                feed.GetEvents().Should().BeEmpty();
            }
        }
    }
}