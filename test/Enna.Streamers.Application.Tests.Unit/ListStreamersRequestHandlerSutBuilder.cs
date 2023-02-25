using Enna.Streamers.Application.Handlers;
using Enna.Streamers.Domain;
using Moq;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class ListStreamersRequestHandlerSutBuilder
    {
        private IStreamerRepository _streamerRepository;

        public ListStreamersRequestHandlerSutBuilder()
        {
            _streamerRepository = new Mock<IStreamerRepository>().Object;
        }

        public ListStreamersRequestHandlerSutBuilder WithNullStreamerRepository()
        {
            _streamerRepository = null!;
            return this;
        }

        public ListStreamersRequestHandlerSutBuilder WithStreamers(params Streamer[] streamers)
        {
            Mock.Get(_streamerRepository)
                .Setup(repository => repository.FindAll())
                .ReturnsAsync(streamers);

            return this;
        }

        public ListStreamersRequestHandler Build()
        {
            return new ListStreamersRequestHandler(
                _streamerRepository);
        }

    }
}
