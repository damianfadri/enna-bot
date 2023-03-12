using Enna.Core.Domain;
using Enna.Streamers.Application.Handlers;
using Enna.Streamers.Domain;
using Moq;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class RemoveStreamerRequestHandlerSutBuilder
    {
        private IStreamerRepository _streamerRepository;

        public RemoveStreamerRequestHandlerSutBuilder()
        {
            _streamerRepository = new Mock<IStreamerRepository>().Object;
        }

        public RemoveStreamerRequestHandlerSutBuilder WithNullStreamerRepository()
        {
            _streamerRepository = null!;
            return this;
        }

        public RemoveStreamerRequestHandlerSutBuilder WithMissingStreamer(Guid streamerId)
        {
            Mock.Get(_streamerRepository)
                .Setup(repository => repository.FindById(streamerId))
                .ReturnsAsync((Streamer)null!);

            return this;
        }

        public RemoveStreamerRequestHandlerSutBuilder WithExistingStreamer(Streamer streamer)
        {
            Mock.Get(_streamerRepository)
                .Setup(repository => repository.FindById(streamer.Id))
                .ReturnsAsync(streamer);

            return this;
        }

        public RemoveStreamerRequestHandlerSutBuilder WithVerifiableStreamerRepository(out Mock<IStreamerRepository> streamerRepository)
        {
            streamerRepository = Mock.Get(_streamerRepository);

            streamerRepository
                .Setup(repository => repository.Remove(It.IsAny<Streamer>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            return this;
        }

        public RemoveStreamerRequestHandler Build()
        {
            return new RemoveStreamerRequestHandler(_streamerRepository);
        }
    }
}
