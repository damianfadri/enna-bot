using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using FluentAssertions;
using Xunit;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class AddFeedRequestHandlerTests
    {
        public class Constructor_Should
        {
            [Fact]
            public void ThrowException_When_StreamerRepositoryIsNull()
            {
                var sut = () =>
                    new AddFeedRequestHandlerSutBuilder()
                        .WithNullStreamerRepository()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ThrowException_When_FeedRepositoryIsNull()
            {
                var sut = () =>
                    new AddFeedRequestHandlerSutBuilder()
                        .WithNullFeedRepository()
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
                    = new AddFeedRequestHandlerSutBuilder()
                        .WithMissingStreamer(streamerId)
                        .Build();

                var sut = () => 
                    handler.Handle(
                        new AddFeedRequest(
                            Guid.NewGuid(), 
                            streamerId, 
                            "console"), 
                        CancellationToken.None);

                await sut.Should().ThrowAsync<InvalidOperationException>();
            }

            [Fact]
            public async Task ThrowException_When_FeedTypeIsInvalid()
            {
                var streamer = new Streamer(
                    Guid.NewGuid(),
                    "Streamer name");

                var handler
                    = new AddFeedRequestHandlerSutBuilder()
                        .WithExistingStreamer(streamer)
                        .Build();

                var sut = () =>
                    handler.Handle(
                        new AddFeedRequest(
                            Guid.NewGuid(),
                            streamer.Id,
                            "consul"),
                        CancellationToken.None);

                await sut.Should().ThrowAsync<InvalidOperationException>();
            }

            [Fact]
            public async Task AddFeedToStreamer()
            {
                var streamer = new Streamer(
                    Guid.NewGuid(),
                    "Streamer name");

                var handler
                    = new AddFeedRequestHandlerSutBuilder()
                        .WithExistingStreamer(streamer)
                        .Build();

                var feedId = Guid.NewGuid();
                await handler.Handle(
                    new AddFeedRequest(
                        feedId,
                        streamer.Id,
                        "console"),
                    CancellationToken.None);

                streamer.Feeds.Should().NotBeEmpty();
                streamer.Feeds.First().Id.Should().Be(feedId);
            }

            [Fact]
            public async Task SaveFeedToDatabase()
            {
                var streamer = new Streamer(
                    Guid.NewGuid(),
                    "Streamer name");

                var handler
                    = new AddFeedRequestHandlerSutBuilder()
                        .WithExistingStreamer(streamer)
                        .Build();

                var feedId = Guid.NewGuid();
                await handler.Handle(
                    new AddFeedRequest(
                        feedId,
                        streamer.Id,
                        "console"),
                    CancellationToken.None);
            }
        }
    }
}