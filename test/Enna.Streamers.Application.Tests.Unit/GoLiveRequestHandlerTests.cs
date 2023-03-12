using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using FluentAssertions;
using Xunit;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class GoLiveRequestHandlerTests
    {
        public class Constructor_Should 
        {
            [Fact]
            public void ThrowException_When_StreamerRepositoryIsNull()
            {
                var sut = () =>
                    new GoLiveRequestHandlerSutBuilder()
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
                    = new GoLiveRequestHandlerSutBuilder()
                        .WithMissingStreamer(streamerId)
                        .Build();

                var sut = () =>
                    handler.Handle(
                        new GoLiveRequest(
                            streamerId,
                            "https://youtube.com/live-link"),
                        CancellationToken.None);

                await sut.Should().ThrowAsync<InvalidOperationException>();
            }

            [Fact]
            public async Task SetStreamerToLive()
            {
                var streamer
                    = new Streamer(
                        Guid.NewGuid(),
                        "Friendly name");

                var handler
                    = new GoLiveRequestHandlerSutBuilder()
                        .WithExistingStreamer(streamer)
                        .Build();

                await handler.Handle(
                    new GoLiveRequest(
                        streamer.Id,
                        "https://youtube.com/live-link"),
                    CancellationToken.None);

                streamer.IsLive.Should().BeTrue();
                streamer.StreamLink
                    .Should().Be("https://youtube.com/live-link");
            }
        }
    }
}
