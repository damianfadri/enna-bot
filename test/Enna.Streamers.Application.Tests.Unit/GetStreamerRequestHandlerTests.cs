using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using FluentAssertions;
using Xunit;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class GetStreamerRequestHandlerTests
    {
        public class Constructor_Should
        {
            [Fact]
            public void ThrowException_When_StreamerRepositoryIsNull()
            {
                var sut = () =>
                    new GetStreamerRequestHandlerSutBuilder()
                        .WithNullStreamerRepository()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }
        }

        public class Handle_Should
        {
            [Fact]
            public async Task ThrowException_When_StreamerIsNotFound()
            {
                var streamerId = Guid.NewGuid();

                var handler
                    = new GetStreamerRequestHandlerSutBuilder()
                        .WithMissingStreamer(streamerId)
                        .Build();

                var sut = () =>
                    handler.Handle(
                        new GetStreamerRequest(
                            streamerId),
                        CancellationToken.None);

                await sut.Should().ThrowAsync<InvalidOperationException>();
            }

            [Fact]
            public async Task ReturnStreamerDto_When_StreamerIsFound()
            {
                var streamer
                    = new Streamer(
                        Guid.NewGuid(),
                        "Streamer name",
                        new Channel(
                            Guid.NewGuid(),
                            "https://youtube.com/channel-link"),
                        new Feed(
                            Guid.NewGuid(),
                            FeedType.Discord,
                            "@link"));

                var handler
                     = new GetStreamerRequestHandlerSutBuilder()
                         .WithExistingStreamer(streamer)
                         .Build();

                var dto 
                    = await handler.Handle(
                        new GetStreamerRequest(
                            streamer.Id),
                        CancellationToken.None);

                dto.Id.Should().Be(streamer.Id);
                dto.Name.Should().Be(streamer.Name);
                dto.Channel.Link.Should().Be("https://youtube.com/channel-link");
                dto.Feed.Type.Should().Be("Discord");
                dto.Feed.MessageTemplate.Should().Be("@link");
            }
        }
    }
}
