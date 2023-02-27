using Enna.Bot.SeedWork;
using Enna.Streamers.Application.Handlers;
using Enna.Streamers.Domain;
using Moq;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class RemoveStreamerRequestHandlerSutBuilder
    {
        private IStreamerRepository _streamerRepository;
        private IUnitOfWork _unitOfWork;

        public RemoveStreamerRequestHandlerSutBuilder()
        {
            _streamerRepository = new Mock<IStreamerRepository>().Object;
            _unitOfWork = new Mock<IUnitOfWork>().Object;
        }

        public RemoveStreamerRequestHandlerSutBuilder WithNullStreamerRepository()
        {
            _streamerRepository = null!;
            return this;
        }

        public RemoveStreamerRequestHandlerSutBuilder WithNullUnitOfWork()
        {
            _unitOfWork = null!;
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

        public RemoveStreamerRequestHandler Build()
        {
            return new RemoveStreamerRequestHandler(
                _streamerRepository, _unitOfWork);
        }
    }
}
