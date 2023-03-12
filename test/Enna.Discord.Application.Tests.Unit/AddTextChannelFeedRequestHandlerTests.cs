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
            public void ThrowException_When_TextChannelRepositoryIsNull()
            {
                var sut = () =>
                    new AddTextChannelFeedRequestHandlerSutBuilder()
                        .WithNullTextChannelRepository()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }
        }

        public class Handle_Should
        {
            [Fact]
            public async Task SaveTextChannelToDatabase()
            {
                var feed = new Feed(
                    Guid.NewGuid(),
                    FeedType.Discord, 
                    "@link");

                var handler =
                    new AddTextChannelFeedRequestHandlerSutBuilder()
                        .Build();

                await handler.Handle(
                    new AddTextChannelFeedRequest(
                            Guid.NewGuid(),
                            feed.Id,
                            0L,
                            1L),
                        CancellationToken.None);
            }
        }
    }
}
