using Enna.Streamers.Application.Handlers;
using Enna.Streamers.Domain;
using Moq;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class GetStreamerRequestHandlerSutBuilder
    {
        private IStreamerRepository _streamerRepository;

        public GetStreamerRequestHandlerSutBuilder()
        {
            _streamerRepository = new Mock<IStreamerRepository>().Object;
        }

        public GetStreamerRequestHandlerSutBuilder WithNullStreamerRepository()
        {
            _streamerRepository = null!;
            return this;
        }

        public GetStreamerRequestHandlerSutBuilder WithMissingStreamer(Guid streamerId)
        {
            Mock.Get(_streamerRepository)
                .Setup(repository => repository.FindById(streamerId))
                .ReturnsAsync((Streamer)null!);

            return this;
        }

        public GetStreamerRequestHandlerSutBuilder WithExistingStreamer(Streamer streamer)
        {
            Mock.Get(_streamerRepository)
                .Setup(repository => repository.FindById(streamer.Id))
                .ReturnsAsync(streamer);

            return this;
        }

        public GetStreamerRequestHandler Build()
        {
            return new GetStreamerRequestHandler(
                _streamerRepository);
        }
    }
}
