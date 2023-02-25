using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using FluentAssertions;
using Xunit;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class ListStreamersRequestHandlerTests
    {
        public class Constructor_Should
        {
            [Fact]
            public void ThrowException_When_StreamerRepositoryIsNull()
            {
                var sut = () =>
                    new ListStreamersRequestHandlerSutBuilder()
                        .WithNullStreamerRepository()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }
        }

        public class Handle_Should
        {
            [Fact]
            public async void ReturnEmptyList_When_StreamerRepositoryIsEmpty()
            {
                var handler =
                    new ListStreamersRequestHandlerSutBuilder()
                        .WithStreamers()
                        .Build();

                var dtos = 
                    await handler.Handle(
                        new ListStreamersRequest(),
                        CancellationToken.None);

                dtos.Should().BeEmpty();
            }

            [Fact]
            public async void ReturnList_When_StreamerRepositoryIsPopulated()
            {
                var streamer1 = new Streamer(Guid.NewGuid(), "Streamer1");
                var streamer2 = new Streamer(Guid.NewGuid(), "Streamer2");

                var handler =
                    new ListStreamersRequestHandlerSutBuilder()
                        .WithStreamers(streamer1, streamer2)
                        .Build();

                var dtos =
                    await handler.Handle(
                        new ListStreamersRequest(),
                        CancellationToken.None);

                dtos.Should().HaveCount(2);
                dtos.Should().Contain(dto => dto.Name == "Streamer1");
                dtos.Should().Contain(dto => dto.Name == "Streamer2");
            }

            [Fact]
            public async void TrimListToFive_When_StreamerRepositoryHasMoreThanFive()
            {
                var streamers = new List<Streamer>();
                for (var i = 0; i < 10; i++)
                {
                    streamers.Add(
                        new Streamer(
                            Guid.NewGuid(), 
                            $"Streamer{i}"));
                }

                var handler =
                    new ListStreamersRequestHandlerSutBuilder()
                        .WithStreamers(streamers.ToArray())
                        .Build();

                var dtos =
                    await handler.Handle(
                        new ListStreamersRequest(),
                        CancellationToken.None);

                dtos.Should().HaveCount(5);
                dtos.Should().Contain(dto => dto.Name == "Streamer3");
                dtos.Should().NotContain(dto => dto.Name == "Streamer7");
            }
        }
    }
}
