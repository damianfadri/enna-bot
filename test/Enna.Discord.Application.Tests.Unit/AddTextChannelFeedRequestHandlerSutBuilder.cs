using Enna.Core.Domain;
using Enna.Discord.Application.Handlers;
using Enna.Discord.Domain;
using Enna.Streamers.Domain;
using Moq;

namespace Enna.Discord.Application.Tests.Unit
{
    public class AddTextChannelFeedRequestHandlerSutBuilder
    {
        private IFeedRepository _feedRepository;
        private ITextChannelFeedRepository _textChannelRepository;
        private IUnitOfWork _unitOfWork;

        public AddTextChannelFeedRequestHandlerSutBuilder()
        {
            _feedRepository = new Mock<IFeedRepository>().Object;
            _textChannelRepository = new Mock<ITextChannelFeedRepository>().Object;
            _unitOfWork = new Mock<IUnitOfWork>().Object;
        }

        public AddTextChannelFeedRequestHandlerSutBuilder WithNullFeedRepository()
        {
            _feedRepository = null!;
            return this;
        }

        public AddTextChannelFeedRequestHandlerSutBuilder WithNullTextChannelRepository()
        {
            _textChannelRepository = null!;
            return this;
        }

        public AddTextChannelFeedRequestHandlerSutBuilder WithNullUnitOfWork()
        {
            _unitOfWork = null!;
            return this;
        }

        public AddTextChannelFeedRequestHandlerSutBuilder WithMissingFeed(Guid feedId)
        {
            Mock.Get(_feedRepository)
                .Setup(repository => repository.FindById(feedId))
                .ReturnsAsync((Feed)null!);

            return this;
        }

        public AddTextChannelFeedRequestHandlerSutBuilder WithExistingFeed(Feed feed)
        {
            Mock.Get(_feedRepository)
                .Setup(repository => repository.FindById(feed.Id))
                .ReturnsAsync(feed);

            return this;
        }

        public AddTextChannelFeedRequestHandler Build()
        {
            return new AddTextChannelFeedRequestHandler(
                _feedRepository,
                _textChannelRepository,
                _unitOfWork);
        }
    }
}
