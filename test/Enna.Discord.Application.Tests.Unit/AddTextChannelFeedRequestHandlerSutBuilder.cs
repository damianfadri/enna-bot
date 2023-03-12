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

        public AddTextChannelFeedRequestHandlerSutBuilder()
        {
            _feedRepository = new Mock<IFeedRepository>().Object;
            _textChannelRepository = new Mock<ITextChannelFeedRepository>().Object;
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

        public AddTextChannelFeedRequestHandlerSutBuilder WithVerifiableTextChannelRepository(out Mock<ITextChannelFeedRepository> textChannelRepository)
        {
            textChannelRepository = Mock.Get(_textChannelRepository);

            textChannelRepository
                .Setup(repository => repository.Add(It.IsAny<TextChannelFeed>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            return this;
        }

        public AddTextChannelFeedRequestHandler Build()
        {
            return new AddTextChannelFeedRequestHandler(
                _feedRepository,
                _textChannelRepository);
        }
    }
}
