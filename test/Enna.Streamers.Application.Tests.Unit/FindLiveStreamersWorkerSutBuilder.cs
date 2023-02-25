using Enna.Streamers.Application.Workers;
using Enna.Streamers.Domain;
using Enna.Streamers.Domain.SeedWork;
using Moq;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class FindLiveStreamersWorkerSutBuilder
    {
        private List<ILinkFetcher> _fetchers;
        private IStreamerRepository _streamerRepository;
        private IDomainEventDispatcher _domainEventDispatcher;

        public FindLiveStreamersWorkerSutBuilder()
        {
            _fetchers = new List<ILinkFetcher>();
            _streamerRepository = new Mock<IStreamerRepository>().Object;
            _domainEventDispatcher = new Mock<IDomainEventDispatcher>().Object;
        }

        public FindLiveStreamersWorkerSutBuilder WithNullStreamerRepository()
        {
            _streamerRepository = null!;
            return this;
        }

        public FindLiveStreamersWorkerSutBuilder WithNullDomainEventDispatcher()
        {
            _domainEventDispatcher = null!;
            return this;
        }

        public FindLiveStreamersWorkerSutBuilder WithStreamers(params Streamer[] streamers)
        {
            Mock.Get(_streamerRepository)
                .Setup(repository => repository.FindAll())
                .ReturnsAsync(streamers);

            return this;
        }

        public FindLiveStreamersWorkerSutBuilder WithFetcherDetectingOffline(string channelLink)
        {
            var mockLinkFetcher = new Mock<ILinkFetcher>().Object;

            Mock.Get(mockLinkFetcher)
                .Setup(fetcher => fetcher.CanFetch(
                    It.Is<Channel>(c => c.Link == channelLink)))
                .Returns(true);

            Mock.Get(mockLinkFetcher)
                .Setup(fetcher => fetcher.Fetch(
                    It.Is<Channel>(c => c.Link == channelLink)))
                .ReturnsAsync((string)null!);

            _fetchers.Add(mockLinkFetcher);

            return this;
        }

        public FindLiveStreamersWorkerSutBuilder WithFetcherDetectingOnline(
            string channelLink,
            string streamLink)
        {
            var mockLinkFetcher = new Mock<ILinkFetcher>().Object;

            Mock.Get(mockLinkFetcher)
                .Setup(fetcher => fetcher.CanFetch(
                    It.Is<Channel>(c => c.Link == channelLink)))
                .Returns(true);

            Mock.Get(mockLinkFetcher)
                .Setup(fetcher => fetcher.Fetch(
                    It.Is<Channel>(c => c.Link == channelLink)))
                .ReturnsAsync(streamLink);

            _fetchers.Add(mockLinkFetcher);

            return this;
        }

        public FindLiveStreamersWorker Build()
        {
            return new FindLiveStreamersWorker(
                _fetchers,
                _streamerRepository,
                _domainEventDispatcher);
        }
    }
}
