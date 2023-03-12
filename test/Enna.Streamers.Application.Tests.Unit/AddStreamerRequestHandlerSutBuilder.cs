using Enna.Core.Domain;
using Enna.Streamers.Application.Handlers;
using Enna.Streamers.Domain;
using Moq;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class AddStreamerRequestHandlerSutBuilder
    {
        private IStreamerRepository _streamerRepository;
        private IChannelRepository _channelRepository;
        private IFeedRepository _feedRepository;
        private List<ILinkFetcher> _fetchers;

        public AddStreamerRequestHandlerSutBuilder() 
        {
            _streamerRepository = new Mock<IStreamerRepository>().Object;
            _channelRepository= new Mock<IChannelRepository>().Object;
            _feedRepository = new Mock<IFeedRepository>().Object;
            _fetchers = new List<ILinkFetcher>();
        }

        public AddStreamerRequestHandlerSutBuilder WithNullStreamerRepository()
        {
            _streamerRepository = null!;
            return this;
        }

        public AddStreamerRequestHandlerSutBuilder WithNullChannelRepository()
        {
            _channelRepository = null!;
            return this;
        }

        public AddStreamerRequestHandlerSutBuilder WithNullFeedRepository()
        {
            _feedRepository = null!;
            return this;
        }

        public AddStreamerRequestHandlerSutBuilder WithLinkFetcherThatCanFetch(string channelLink)
        {
            var mockLinkFetcher = new Mock<ILinkFetcher>();

            mockLinkFetcher
                .Setup(fetcher => fetcher.CanFetch(channelLink))
                .Returns(true);

            _fetchers.Add(mockLinkFetcher.Object);

            return this;
        }

        public AddStreamerRequestHandlerSutBuilder WithVerifiableChannelRepository(out Mock<IChannelRepository> channelRepository)
        {
            channelRepository = Mock.Get(_channelRepository);

            channelRepository
                .Setup(repository => repository.Add(It.IsAny<Channel>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            return this;
        }

        public AddStreamerRequestHandlerSutBuilder WithVerifiableFeedRepository(out Mock<IFeedRepository> feedRepository)
        {
            feedRepository = Mock.Get(_feedRepository);

            feedRepository
                .Setup(repository => repository.Add(It.IsAny<Feed>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            return this;
        }

        public AddStreamerRequestHandlerSutBuilder WithVerifiableStreamerRepository(out Mock<IStreamerRepository> streamerRepository)
        {
            streamerRepository = Mock.Get(_streamerRepository);

            streamerRepository
                .Setup(repository => repository.Add(It.IsAny<Streamer>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            return this;
        }

        public AddStreamerRequestHandler Build()
        {
            return new AddStreamerRequestHandler(
                _streamerRepository,
                _channelRepository,
                _feedRepository,
                _fetchers);
        }
    }
}
