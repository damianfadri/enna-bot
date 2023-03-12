using Discord.WebSocket;
using Enna.Discord.Application.Handlers;
using Enna.Discord.Domain;
using Enna.Streamers.Domain;
using Moq;

namespace Enna.Discord.Application.Tests.Unit
{
    public class TextChannelFeedNotifiedEventHandlerSutBuilder
    {
        private DiscordSocketClient _client;
        private IFeedRepository _feedRepository;
        private ITextChannelFeedRepository _textChannelRepository;

        public TextChannelFeedNotifiedEventHandlerSutBuilder()
        {
            _client = new Mock<DiscordSocketClient>().Object;
            _feedRepository = new Mock<IFeedRepository>().Object;
            _textChannelRepository = new Mock<ITextChannelFeedRepository>().Object;
        }

        public TextChannelFeedNotifiedEventHandlerSutBuilder WithNullClient()
        {
            _client = null!;
            return this;
        }

        public TextChannelFeedNotifiedEventHandlerSutBuilder WithNullFeedRepository()
        {
            _feedRepository = null!;
            return this;
        }

        public TextChannelFeedNotifiedEventHandlerSutBuilder WithNullTextChannelRepository()
        {
            _textChannelRepository = null!;
            return this;
        }

        public TextChannelFeedNotifiedEventHandlerSutBuilder WithMissingFeed(Guid feedId)
        {
            Mock.Get(_feedRepository)
                .Setup(repository => repository.FindById(feedId))
                .ReturnsAsync((Feed)null!);

            return this;
        }

        public TextChannelFeedNotifiedEventHandlerSutBuilder WithMissingTextChannelDetails(Guid feedId)
        {
            Mock.Get(_textChannelRepository)
                .Setup(repository => repository.FindByFeedId(feedId))
                .ReturnsAsync((TextChannelFeed)null!);

            return this;
        }

        public TextChannelFeedNotifiedEventHandlerSutBuilder WithExistingFeed(
            Feed feed, TextChannelFeed textChannel)
        {
            Mock.Get(_feedRepository)
                .Setup(repository => repository.FindById(feed.Id))
                .ReturnsAsync(feed);

            Mock.Get(_textChannelRepository)
                .Setup(repository => repository.FindByFeedId(textChannel.Id))
                .ReturnsAsync(textChannel);

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
                _client, 
                _feedRepository,
                _textChannelRepository);
        }
    }
}
