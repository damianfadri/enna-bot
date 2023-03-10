using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using FluentAssertions;
using Xunit;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class ListFeedsRequestHandlerTests
    {
        public class Constructor_Should
        {
            [Fact]
            public void ThrowException_When_FeedRepositoryIsNull()
            {
                var sut = () =>
                    new ListFeedsRequestHandlerSutBuilder()
                        .WithNullFeedRepository()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }
        }

        public class Handle_Should
        {
            [Fact]
            public async void ReturnEmptyList_When_FeedRepositoryIsEmpty()
            {
                var streamerId = Guid.NewGuid();

                var handler =
                    new ListFeedsRequestHandlerSutBuilder()
                        .WithFeeds(streamerId)
                        .Build();

                var dtos =
                    await handler.Handle(
                        new ListFeedsRequest(streamerId),
                        CancellationToken.None);

                dtos.Should().BeEmpty();
            }

            [Fact]
            public async void ReturnList_When_StreamerRepositoryIsPopulated()
            {
                var feed1 = new Feed(Guid.NewGuid(), FeedType.Console, "@link");
                var feed2 = new Feed(Guid.NewGuid(), FeedType.Discord, "@link");

                var streamerId = Guid.NewGuid();

                var handler =
                    new ListFeedsRequestHandlerSutBuilder()
                        .WithFeeds(streamerId, feed1, feed2)
                        .Build();

                var dtos =
                    await handler.Handle(
                        new ListFeedsRequest(streamerId),
                        CancellationToken.None);

                dtos.Should().HaveCount(2);

                dtos.Should().Contain(
                    dto => dto.Type == "Console");
                dtos.Should().Contain(
                    dto => dto.Type == "Discord");
            }

            [Fact]
            public async void TrimListToFive_When_StreamerRepositoryHasMoreThanFive()
            {
                var feeds = new List<Feed>();
                for (var i = 0; i < 10; i++)
                {
                    feeds.Add(
                        new Feed(
                            Guid.NewGuid(),
                            FeedType.Console, 
                            "@link"));
                }

                var streamerId = Guid.NewGuid();

                var handler =
                    new ListFeedsRequestHandlerSutBuilder()
                        .WithFeeds(streamerId, feeds.ToArray())
                        .Build();

                var dtos =
                    await handler.Handle(
                        new ListFeedsRequest(streamerId),
                        CancellationToken.None);

                dtos.Should().HaveCount(5);
                dtos.Should().Contain(dto => dto.Type == "Console");
                dtos.Should().NotContain(dto => dto.Type == "Discord");
            }
        }
    }
}
