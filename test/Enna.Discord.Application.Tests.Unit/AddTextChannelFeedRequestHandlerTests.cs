using Enna.Discord.Application.Contracts;
using Enna.Streamers.Domain;
using FluentAssertions;
using Xunit;

namespace Enna.Discord.Application.Tests.Unit
{
    public class AddTextChannelFeedRequestHandlerTests
    {
        public class Constructor_Should
        {
            [Fact]
            public void ThrowException_When_FeedRepositoryIsNull()
            {
                var sut = () =>
                    new AddTextChannelFeedRequestHandlerSutBuilder()
                        .WithNullFeedRepository()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ThrowException_When_TextChannelRepositoryIsNull()
            {
                var sut = () =>
                    new AddTextChannelFeedRequestHandlerSutBuilder()
                        .WithNullTextChannelRepository()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ThrowException_When_UnitOfWorkIsNull()
            {
                var sut = () =>
                    new AddTextChannelFeedRequestHandlerSutBuilder()
                        .WithNullUnitOfWork()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }
        }

        public class Handle_Should
        {
            [Fact]
            public async Task ThrowException_When_FeedIsNotFound()
            {
                var feedId = Guid.NewGuid();

                var handler =
                    new AddTextChannelFeedRequestHandlerSutBuilder()
                        .WithMissingFeed(feedId)
                        .Build();

                var sut = () =>
                    handler.Handle(
                        new AddTextChannelFeedRequest(
                            feedId, 
                            0L, 
                            1L),
                        CancellationToken.None);

                await sut.Should().ThrowAsync<InvalidOperationException>();
            }

            [Fact]
            public async Task ThrowException_When_FeedIsNotTextChannel()
            {
                var feed = new Feed(
                    Guid.NewGuid(), 
                    FeedType.Console);

                var handler =
                    new AddTextChannelFeedRequestHandlerSutBuilder()
                        .WithExistingFeed(feed)
                        .Build();

                var sut = () =>
                    handler.Handle(
                        new AddTextChannelFeedRequest(
                            feed.Id,
                            0L,
                            1L),
                        CancellationToken.None);

                await sut.Should().ThrowAsync<InvalidOperationException>();
            }

            [Fact]
            public async Task SaveTextChannelToDatabase()
            {
                var feed = new Feed(
                    Guid.NewGuid(),
                    FeedType.Discord);

                var handler =
                    new AddTextChannelFeedRequestHandlerSutBuilder()
                        .WithExistingFeed(feed)
                        .Build();

                await handler.Handle(
                    new AddTextChannelFeedRequest(
                            feed.Id,
                            0L,
                            1L),
                        CancellationToken.None);
            }
        }
    }
}
