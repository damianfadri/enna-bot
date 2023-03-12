using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using FluentAssertions;
using Xunit;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class GoOfflineRequestHandlerTests
    {
        public class Constructor_Should
        {
            [Fact]
            public void ThrowException_When_StreamerRepositoryIsNull()
            {
                var sut = () =>
                    new GoOfflineRequestHandlerSutBuilder()
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
                    = new GoOfflineRequestHandlerSutBuilder()
                        .WithMissingStreamer(streamerId)
                        .Build();

                var sut = () => 
                    handler.Handle(
                        new GoOfflineRequest(streamerId),
                        CancellationToken.None);

                await sut.Should().ThrowAsync<InvalidOperationException>();
            }

            [Fact]
            public async Task SetStreamerToOffline()
            {
                var streamer
                    = new Streamer(
                        Guid.NewGuid(),
                        "Friendly name");

                streamer.GoLive("https://youtube.com/live-link");

                var handler
                    = new GoOfflineRequestHandlerSutBuilder()
                        .WithExistingStreamer(streamer)
                        .Build();

                await handler.Handle(
                    new GoOfflineRequest(streamer.Id),
                    CancellationToken.None);

                streamer.IsLive.Should().BeFalse();
            }
        }
    }
}
