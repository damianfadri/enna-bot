using Enna.Core.Domain;
using Enna.Streamers.Application.Handlers;
using Enna.Streamers.Domain;
using Moq;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class AddFeedRequestHandlerSutBuilder
    {
        private IStreamerRepository _streamerRepository;
        private IFeedRepository _feedRepository;
        private IUnitOfWork _unitOfWork;

        public AddFeedRequestHandlerSutBuilder()
        {
            _streamerRepository = new Mock<IStreamerRepository>().Object;
            _feedRepository = new Mock<IFeedRepository>().Object;
            _unitOfWork = new Mock<IUnitOfWork>().Object;
        }

        public AddFeedRequestHandlerSutBuilder WithNullStreamerRepository()
        {
            _streamerRepository = null!;
            return this;
        }

        public AddFeedRequestHandlerSutBuilder WithNullFeedRepository()
        {
            _feedRepository = null!;
            return this;
        }

        public AddFeedRequestHandlerSutBuilder WithNullUnitOfWork()
        {
            _unitOfWork = null!;
            return this;
        }

        public AddFeedRequestHandlerSutBuilder WithMissingStreamer(Guid streamerId)
        {
            Mock.Get(_streamerRepository)
                .Setup(repository => repository.FindById(streamerId))
                .ReturnsAsync((Streamer)null!);

            return this;
        }

        public AddFeedRequestHandlerSutBuilder WithExistingStreamer(Streamer streamer)
        {
            Mock.Get(_streamerRepository)
                .Setup(repository => repository.FindById(streamer.Id))
                .ReturnsAsync(streamer);

            return this;
        }

        public AddFeedRequestHandler Build()
        {
            return new AddFeedRequestHandler(
                _streamerRepository,
                _feedRepository,
                _unitOfWork);
        }
    }
}
