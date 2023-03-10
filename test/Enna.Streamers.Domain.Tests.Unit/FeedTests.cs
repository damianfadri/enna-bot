using Enna.Streamers.Domain;
using Enna.Streamers.Domain.Events;
using Xunit;

namespace Enna.Streamers.Domain.Tests.Unit
{
    public class FeedTests
    {
        public class Constructor_Should
        {
            [Fact]
            public void SetProperties()
            {
                var id = Guid.NewGuid();
                var type = FeedType.Console;

                var feed = new Feed(id, FeedType.Console, "@link");

                Assert.Equal(id, feed.Id);
                Assert.Equal(type, feed.Type);
            }

            [Fact]
            public void BroadcastFeedCreatedEvent()
            {
                var feed = new Feed(Guid.NewGuid(), FeedType.Console, "@link");

                var @event = feed.GetEvents().Last();

                Assert.IsType<FeedCreatedEvent>(@event);
                Assert.Equal(feed.Id, ((FeedCreatedEvent)@event).Feed.Id);
            }
        }

        public class Notify_Should
        {
            [Fact]
            public void UpdateLastNotifiedUtc_When_FeedIsNotYetNotified()
            {
                var channel = new Channel(Guid.NewGuid(), "https://youtube.com/some-channel");
                channel.GoLive("https://youtube.com/live-link");

                var feed = new Feed(Guid.NewGuid(), FeedType.Console, "@link");
                var oldLastNotifiedutc = feed.LastNotifiedUtc;

                feed.Notify(channel);

                Assert.True(feed.LastNotifiedUtc > oldLastNotifiedutc);
            }

            [Fact]
            public void BroadcastFeedNotifiedEvent_When_FeedIsNotYetNotified()
            {
                var channel = new Channel(Guid.NewGuid(), "https://youtube.com/some-channel");
                channel.GoLive("https://youtube.com/live-link");

                var feed = new Feed(Guid.NewGuid(), FeedType.Console, "@link");
                var oldLastNotifiedutc = feed.LastNotifiedUtc;

                feed.Notify(channel);

                var @event = feed.GetEvents().Last();

                Assert.IsType<FeedNotifiedEvent>(@event);
                Assert.Equal(feed.Id, ((FeedNotifiedEvent)@event).Feed.Id);
                Assert.Equal(channel.Id, ((FeedNotifiedEvent)@event).Channel.Id);
            }

            [Fact]
            public void DoNothing_When_FeedIsAlreadyNotified()
            {
                var channel = new Channel(Guid.NewGuid(), "https://youtube.com/some-channel");
                channel.GoLive("https://youtube.com/live-link");

                var feed = new Feed(Guid.NewGuid(), FeedType.Console, "@link");

                feed.Notify(channel);
                feed.ClearEvents();

                var oldLastNotifiedUtc = feed.LastNotifiedUtc;

                feed.Notify(channel);

                Assert.Equal(oldLastNotifiedUtc, feed.LastNotifiedUtc);
                Assert.Empty(feed.GetEvents());
            }
        }
    }
}