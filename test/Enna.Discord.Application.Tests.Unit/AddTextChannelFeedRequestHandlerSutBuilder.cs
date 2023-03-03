using Enna.Core.Domain;
using Enna.Discord.Application.Handlers;
using Enna.Discord.Domain;
using Enna.Streamers.Domain;
using Moq;

namespace Enna.Discord.Application.Tests.Unit
{
    public class AddTextChannelFeedRequestHandlerSutBuilder
    {
        private ITextChannelFeedRepository _textChannelRepository;

        public AddTextChannelFeedRequestHandlerSutBuilder()
        {
            _textChannelRepository = new Mock<ITextChannelFeedRepository>().Object;
        }

        public AddTextChannelFeedRequestHandlerSutBuilder WithNullTextChannelRepository()
        {
            _textChannelRepository = null!;
            return this;
        }

        public AddTextChannelFeedRequestHandler Build()
        {
            return new AddTextChannelFeedRequestHandler(_textChannelRepository);
        }
    }
}
