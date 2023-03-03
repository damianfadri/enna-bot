using Enna.Streamers.Application.Contracts;
using FluentAssertions;
using Xunit;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class AddStreamerRequestHandlerTests
    {
        public class Constructor_Should
        {
            [Fact]
            public void ThrowException_When_StreamerRepositoryIsNull()
            {
                var sut = () =>
                    new AddStreamerRequestHandlerSutBuilder()
                        .WithNullStreamerRepository()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ThrowException_When_ChannelRepositoryIsNull()
            {
                var sut = () =>
                    new AddStreamerRequestHandlerSutBuilder()
                        .WithNullChannelRepository()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }
        }

        public class Handle_Should
        {
            [Fact]
            public async Task SaveStreamerToDatabase()
            {
                var handler =
                    new AddStreamerRequestHandlerSutBuilder()
                        .Build();

                await handler.Handle(
                    new AddStreamerRequest(
                        Guid.NewGuid(), 
                        "Streamer", 
                        "https://youtube.com/channel-link"),
                    CancellationToken.None);
            }
        }
    }
}
