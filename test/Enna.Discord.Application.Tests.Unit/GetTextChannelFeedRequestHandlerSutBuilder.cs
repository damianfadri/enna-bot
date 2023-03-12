using Enna.Discord.Application.Handlers;
using Enna.Discord.Domain;
using Enna.Streamers.Domain;
using Moq;

namespace Enna.Discord.Application.Tests.Unit
{
    public class GetTextChannelFeedRequestHandlerSutBuilder
    {
        private ITextChannelFeedRepository _textChannelRepository;

        public GetTextChannelFeedRequestHandlerSutBuilder()
        {
            _textChannelRepository = new Mock<ITextChannelFeedRepository>().Object;
        }

        public GetTextChannelFeedRequestHandlerSutBuilder WithNullTextChannelRepository()
        {
            _textChannelRepository = null!;
            return this;
        }

        public GetTextChannelFeedRequestHandlerSutBuilder WithMissingFeed(Guid feedId)
        {
            Mock.Get(_textChannelRepository)
                .Setup(repository => repository.FindByFeedId(feedId))
                .ReturnsAsync((TextChannelFeed)null!);

            return this;
        }

        public GetTextChannelFeedRequestHandlerSutBuilder WithExistingFeed(TextChannelFeed feed)
        {
            Mock.Get(_textChannelRepository)
                .Setup(repository => repository.FindByFeedId(feed.Id))
                .ReturnsAsync(feed);

            return this;
        }

        public GetTextChannelFeedRequestHandler Build()
        {
            return new GetTextChannelFeedRequestHandler(
                _textChannelRepository);
        }
    }
}
