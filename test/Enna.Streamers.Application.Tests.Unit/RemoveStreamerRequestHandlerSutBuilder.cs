using Enna.Core.Domain;
using Enna.Streamers.Application.Handlers;
using Enna.Streamers.Domain;
using Moq;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class RemoveStreamerRequestHandlerSutBuilder
    {
        private IStreamerRepository _streamerRepository;
        private IChannelRepository _channelRepository;
        private IFeedRepository _feedRepository;

        public RemoveStreamerRequestHandlerSutBuilder()
        {
            _streamerRepository = new Mock<IStreamerRepository>().Object;
            _channelRepository = new Mock<IChannelRepository>().Object;
            _feedRepository = new Mock<IFeedRepository>().Object;
        }

        public RemoveStreamerRequestHandlerSutBuilder WithNullStreamerRepository()
        {
            _streamerRepository = null!;
            return this;
        }

        public RemoveStreamerRequestHandlerSutBuilder WithNullChannelRepository()
        {
            _channelRepository = null!;
            return this;
        }

        public RemoveStreamerRequestHandlerSutBuilder WithNullFeedRepository()
        {
            _feedRepository = null!;
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

        public RemoveStreamerRequestHandlerSutBuilder WithVerifiableChannelRepository(out Mock<IChannelRepository> channelRepository)
        {
            channelRepository = Mock.Get(_channelRepository);

            channelRepository
                .Setup(repository => repository.Remove(It.IsAny<Channel>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            return this;
        }

        public RemoveStreamerRequestHandlerSutBuilder WithVerifiableFeedRepository(out Mock<IFeedRepository> feedRepository)
        {
            feedRepository = Mock.Get(_feedRepository);

            feedRepository
                .Setup(repository => repository.Remove(It.IsAny<Feed>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            return this;
        }

        public RemoveStreamerRequestHandler Build()
        {
            return new RemoveStreamerRequestHandler(
                _streamerRepository,
                _channelRepository,
                _feedRepository);
        }
    }
}
