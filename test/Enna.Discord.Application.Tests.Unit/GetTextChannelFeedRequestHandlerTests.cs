using Enna.Discord.Application.Contracts;
using Enna.Discord.Domain;
using FluentAssertions;
using Xunit;

namespace Enna.Discord.Application.Tests.Unit
{
    public class GetTextChannelFeedRequestHandlerTests
    {
        public class Constructor_Should
        {
            [Fact]
            public void ThrowException_When_TextChannelRepositoryIsNull()
            {
                var sut = () =>
                    new GetTextChannelFeedRequestHandlerSutBuilder()
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
                    = new GetTextChannelFeedRequestHandlerSutBuilder()
                        .WithMissingFeed(feedId)
                        .Build();

                var sut = () =>
                    handler.Handle(
                        new GetTextChannelFeedRequest(feedId),
                        CancellationToken.None);

                await sut.Should().ThrowAsync<InvalidOperationException>();
            }

            [Fact]
            public async Task ReturnTextChannelFeedDto()
            {
                var feed 
                    = new TextChannelFeed(
                        Guid.NewGuid(),
                        0L,
                        1L);

                var handler 
                    = new GetTextChannelFeedRequestHandlerSutBuilder()
                        .WithExistingFeed(feed)
                        .Build();

                var dto 
                    = await handler.Handle(
                        new GetTextChannelFeedRequest(
                            feed.Id),
                        CancellationToken.None);

                dto.Id.Should().Be(feed.Id);
                dto.GuildId.Should().Be(feed.Guild);
                dto.ChannelId.Should().Be(feed.Channel);
            }
        }
    }
}
