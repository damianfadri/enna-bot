﻿using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using FluentAssertions;
using Xunit;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class RemoveStreamerRequestHandlerTests
    {
        public class Constructor_Should
        {
            [Fact]
            public void ThrowException_When_StreamerRepositoryIsNull()
            {
                var sut = () =>
                    new RemoveStreamerRequestHandlerSutBuilder()
                        .WithNullStreamerRepository()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ThrowException_When_ChannelRepositoryIsNull()
            {
                var sut = () =>
                    new RemoveStreamerRequestHandlerSutBuilder()
                        .WithNullChannelRepository()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ThrowException_When_FeedRepositoryIsNull()
            {
                var sut = () =>
                    new RemoveStreamerRequestHandlerSutBuilder()
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

                var handler =
                    new RemoveStreamerRequestHandlerSutBuilder()
                        .WithMissingStreamer(streamerId)
                        .Build();

                var sut = () => 
                    handler.Handle(
                        new RemoveStreamerRequest(streamerId),
                        CancellationToken.None);

                await sut.Should().ThrowAsync<InvalidOperationException>();
            }

            [Fact]
            public async Task RemoveStreamerFromDatabase()
            {
                var streamer = new Streamer(
                    Guid.NewGuid(),
                    "Streamer name");

                var handler =
                    new RemoveStreamerRequestHandlerSutBuilder()
                        .WithExistingStreamer(streamer)
                        .WithVerifiableStreamerRepository(out var streamerRepository)
                        .WithVerifiableChannelRepository(out var channelRepository)
                        .WithVerifiableFeedRepository(out var feedRepository)
                        .Build();

                await handler.Handle(
                    new RemoveStreamerRequest(streamer.Id),
                    CancellationToken.None);

                streamerRepository.Verify();
                channelRepository.Verify();
                feedRepository.Verify();
            }
        }
    }
}
