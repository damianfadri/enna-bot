using Enna.Discord.Domain;
using Enna.Streamers.Domain;
using Enna.Streamers.Domain.Events;
using FluentAssertions;
using Xunit;

namespace Enna.Discord.Application.Tests.Unit
{
    public class TextChannelFeedNotifiedEventHandlerTests
    {
        public class Constructor_Should
        {
            [Fact]
            public void ThrowException_When_DiscordClientIsNull()
            {
                var sut = () =>
                    new TextChannelFeedNotifiedEventHandlerSutBuilder()
                        .WithNullClient()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ThrowException_When_FeedRepositoryIsNull()
            {
                var sut = () =>
                    new TextChannelFeedNotifiedEventHandlerSutBuilder()
                        .WithNullTextChannelRepository()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }
        }

        public class Handle_Should
        {
            [Fact]
            public async Task IgnoreNonDiscordFeedTypes()
            {
                var handler =
                    new TextChannelFeedNotifiedEventHandlerSutBuilder()
                        .Build();

                var feed = new Feed(Guid.NewGuid(), FeedType.Console);
                var channel = new Channel(
                    Guid.NewGuid(),
                    "https://youtube.com/channel-link");

                channel.GoLive("https://youtube.com/stream-link");

                await handler.Handle(
                    new FeedNotifiedEvent(
                        feed,
                        channel),
                    CancellationToken.None);
            }

            [Fact]
            public async Task ThrowException_When_FeedIsNotFound()
            {
                var feedId = Guid.NewGuid();

                var handler =
                    new TextChannelFeedNotifiedEventHandlerSutBuilder()
                        .WithMissingFeed(feedId)
                        .Build();

                var feed = new Feed(feedId, FeedType.Discord);
                var channel = new Channel(
                    Guid.NewGuid(),
                    "https://youtube.com/channel-link");

                channel.GoLive("https://youtube.com/stream-link");

                var sut = () =>
                    handler.Handle(
                        new FeedNotifiedEvent(
                            feed,
                            channel),
                        CancellationToken.None);

                await sut.Should().ThrowAsync<InvalidOperationException>();
            }

            [Fact]
            public async Task ThrowException_When_GuildIsNotFound()
            {
                var textChannelFeed =
                    new TextChannelFeed(
                        Guid.NewGuid(),
                        1L,
                        1L);

                var handler =
                    new TextChannelFeedNotifiedEventHandlerSutBuilder()
                        .WithExistingFeed(textChannelFeed)
                        .WithMissingGuild(textChannelFeed.Guild)
                        .Build();

                var feed = new Feed(textChannelFeed.Id, FeedType.Discord);
                var channel = new Channel(
                    Guid.NewGuid(),
                    "https://youtube.com/channel-link");

                channel.GoLive("https://youtube.com/stream-link");

                var sut = () =>
                    handler.Handle(
                        new FeedNotifiedEvent(
                            feed,
                            channel),
                        CancellationToken.None);

                await sut.Should().ThrowAsync<InvalidOperationException>();
            }
        }
    }
}
