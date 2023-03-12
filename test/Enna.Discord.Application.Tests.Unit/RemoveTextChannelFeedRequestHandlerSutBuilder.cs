using Enna.Discord.Application.Handlers;
using Enna.Discord.Domain;
using Moq;

namespace Enna.Discord.Application.Tests.Unit
{
    public class RemoveTextChannelFeedRequestHandlerSutBuilder
    {
        private ITextChannelFeedRepository _textChannelRepository;

        public RemoveTextChannelFeedRequestHandlerSutBuilder()
        {
            _textChannelRepository = new Mock<ITextChannelFeedRepository>().Object;
        }

        public RemoveTextChannelFeedRequestHandlerSutBuilder WithNullTextChannelRepository()
        {
            _textChannelRepository = null!;
            return this;
        }

        public RemoveTextChannelFeedRequestHandlerSutBuilder WithMissingFeed(Guid feedId)
        {
            Mock.Get(_textChannelRepository)
                .Setup(repository => repository.FindByFeedId(feedId))
                .ReturnsAsync((TextChannelFeed)null!);

            return this;
        }

        public RemoveTextChannelFeedRequestHandlerSutBuilder WithExistingFeed(TextChannelFeed feed)
        {
            Mock.Get(_textChannelRepository)
                .Setup(repository => repository.FindByFeedId(feed.Id))
                .ReturnsAsync(feed);

            return this;
        }

        public RemoveTextChannelFeedRequestHandlerSutBuilder WithVerifiableTextChannelRepository(
            out Mock<ITextChannelFeedRepository> textChannelRepository)
        {
            textChannelRepository = Mock.Get(_textChannelRepository);

            textChannelRepository
                .Setup(repository => repository.Remove(It.IsAny<TextChannelFeed>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            return this;
        }

        public RemoveTextChannelFeedRequestHandler Build()
        {
            return new RemoveTextChannelFeedRequestHandler(
                _textChannelRepository);
        }
    }
}
