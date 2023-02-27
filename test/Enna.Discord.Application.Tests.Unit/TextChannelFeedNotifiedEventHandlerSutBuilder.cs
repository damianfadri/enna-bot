using Discord.WebSocket;
using Enna.Discord.Application.Handlers;
using Enna.Discord.Domain;
using Moq;

namespace Enna.Discord.Application.Tests.Unit
{
    public class TextChannelFeedNotifiedEventHandlerSutBuilder
    {
        private DiscordSocketClient _client;
        private ITextChannelFeedRepository _textChannelRepository;

        public TextChannelFeedNotifiedEventHandlerSutBuilder()
        {
            _client = new Mock<DiscordSocketClient>().Object;
            _textChannelRepository = new Mock<ITextChannelFeedRepository>().Object;
        }

        public TextChannelFeedNotifiedEventHandlerSutBuilder WithNullClient()
        {
            _client = null!;
            return this;
        }

        public TextChannelFeedNotifiedEventHandlerSutBuilder WithNullTextChannelRepository()
        {
            _textChannelRepository = null!;
            return this;
        }

        public TextChannelFeedNotifiedEventHandlerSutBuilder WithMissingFeed(Guid feedId)
        {
            Mock.Get(_textChannelRepository)
                .Setup(repository => repository.FindById(feedId))
                .ReturnsAsync((TextChannelFeed)null!);

            return this;
        }

        public TextChannelFeedNotifiedEventHandlerSutBuilder WithExistingFeed(
            TextChannelFeed feed)
        {
            Mock.Get(_textChannelRepository)
                .Setup(repository => repository.FindById(feed.Id))
                .ReturnsAsync(feed);

            return this;
        }

        public TextChannelFeedNotifiedEventHandlerSutBuilder WithMissingGuild(ulong guild)
        {
            Mock.Get(_client)
                .Setup(client => client.GetGuild(guild))
                .Returns((SocketGuild)null!);

            return this;
        }

        public TextChannelFeedNotifiedEventHandler Build()
        {
            return new TextChannelFeedNotifiedEventHandler(
                _client, _textChannelRepository);
        }
    }
}
