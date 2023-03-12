using Enna.Discord.Application.Contracts;
using Enna.Discord.Domain;
using FluentAssertions;
using Xunit;

namespace Enna.Discord.Application.Tests.Unit
{
    public class RemoveTextChannelFeedRequestHandlerTests
    {
        public class Constructor_Should
        {
            [Fact]
            public void ThrowException_When_TextChannelRepositoryIsNull()
            {
                var sut = () =>
                    new RemoveTextChannelFeedRequestHandlerSutBuilder()
                        .WithNullTextChannelRepository()
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

                var handler
                    = new RemoveTextChannelFeedRequestHandlerSutBuilder()
                        .WithMissingFeed(feedId)
                        .Build();

                var sut = () =>
                    handler.Handle(
                        new RemoveTextChannelFeedRequest(feedId),
                        CancellationToken.None);

                await sut.Should().ThrowAsync<InvalidOperationException>();
            }

            [Fact]
            public async Task RemoveFeedFromDatabase()
            {
                var feed
                    = new TextChannelFeed(
                        Guid.NewGuid(),
                        0L,
                        1L);

                var handler
                    = new RemoveTextChannelFeedRequestHandlerSutBuilder()
                        .WithExistingFeed(feed)
                        .WithVerifiableTextChannelRepository(
                            out var textChannelRepository)
                        .Build();

                await handler.Handle(
                    new RemoveTextChannelFeedRequest(feed.Id),
                    CancellationToken.None);

                textChannelRepository.Verify();
            }
        }
    }
}
