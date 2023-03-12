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
