using Enna.Streamers.Application.Contracts;
using FluentAssertions;
using Moq;
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

            [Fact]
            public void ThrowException_When_FeedRepositoryIsNull()
            {
                var sut = () =>
                    new AddStreamerRequestHandlerSutBuilder()
                        .WithNullFeedRepository()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }
        }

        public class Handle_Should
        {
            [Fact]
            public async Task ThrowException_When_ChannelLinkIsInvalid()
            {
                var handler =
                    new AddStreamerRequestHandlerSutBuilder()
                        .Build();

                var sut = () =>
                    handler.Handle(
                        new AddStreamerRequest(
                            Guid.NewGuid(),
                            "Friendly name",
                            "https://youtube.com/channel-link",
                            "console",
                            "@link"),
                        CancellationToken.None);

                await sut.Should().ThrowAsync<InvalidOperationException>();
            }

            [Fact]
            public async Task ThrowException_When_FeedTypeIsInvalid()
            {
                var handler =
                    new AddStreamerRequestHandlerSutBuilder()
                        .WithLinkFetcherThatCanFetch(
                            "https://youtube.com/channel-link")
                        .Build();

                var sut = () =>
                    handler.Handle(
                        new AddStreamerRequest(
                            Guid.NewGuid(),
                            "Friendly name",
                            "https://youtube.com/channel-link",
                            "consul",
                            "@link"),
                        CancellationToken.None);

                await sut.Should().ThrowAsync<InvalidOperationException>();
            }

            [Fact]
            public async Task SaveStreamerToDatabase()
            {
                var handler =
                    new AddStreamerRequestHandlerSutBuilder()
                        .WithLinkFetcherThatCanFetch(
                            "https://youtube.com/channel-link")
                        .WithVerifiableChannelRepository(out var channelRepository)
                        .WithVerifiableFeedRepository(out var feedRepository)
                        .WithVerifiableStreamerRepository(out var streamerRepository)
                        .Build();

                await handler.Handle(
                    new AddStreamerRequest(
                        Guid.NewGuid(), 
                        "Friendly name", 
                        "https://youtube.com/channel-link",
                        "console",
                        "@link"),
                    CancellationToken.None);

                channelRepository.Verify();
                feedRepository.Verify();
                streamerRepository.Verify();
            }
        }
    }
}
