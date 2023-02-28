using Enna.Streamers.Application.Handlers;
using Enna.Streamers.Domain;
using Moq;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class ListFeedsRequestHandlerSutBuilder
    {
        private IFeedRepository _feedRepository;

        public ListFeedsRequestHandlerSutBuilder()
        {
            _feedRepository = new Mock<IFeedRepository>().Object;
        }

        public ListFeedsRequestHandlerSutBuilder WithNullFeedRepository()
        {
            _feedRepository = null!;
            return this;
        }

        public ListFeedsRequestHandlerSutBuilder WithFeeds(
            Guid streamerId, 
            params Feed[] feeds)
        {
            Mock.Get(_feedRepository)
                .Setup(repository => repository.FindByStreamerId(streamerId))
                .ReturnsAsync(feeds);

            return this;
        }

        public ListFeedsRequestHandler Build()
        {
            return new ListFeedsRequestHandler(
                _feedRepository);
        }
    }
}
