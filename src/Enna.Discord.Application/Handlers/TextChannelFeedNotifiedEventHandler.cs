using Discord;
using Discord.WebSocket;
using Enna.Discord.Domain;
using Enna.Streamers.Domain;
using Enna.Streamers.Domain.Events;
using MediatR;

namespace Enna.Discord.Application.Handlers
{
    public class TextChannelFeedNotifiedEventHandler
        : INotificationHandler<FeedNotifiedEvent>
    {
        private const string DEFAULT_TEMPLATE = "@link";

        private readonly DiscordSocketClient _client;
        private readonly IFeedRepository _feedRepository;
        private readonly ITextChannelFeedRepository _textChannelRepository;

        public TextChannelFeedNotifiedEventHandler(
            DiscordSocketClient client,
            IFeedRepository feedRepository,
            ITextChannelFeedRepository textChannelRepository)
        {
            ArgumentNullException.ThrowIfNull(client);
            ArgumentNullException.ThrowIfNull(feedRepository);
            ArgumentNullException.ThrowIfNull(textChannelRepository);

            _client = client;
            _feedRepository = feedRepository;
            _textChannelRepository = textChannelRepository;
        }

        public async Task Handle(
            FeedNotifiedEvent notification,
            CancellationToken cancellationToken)
        {
            if (notification.Feed.Type != FeedType.Discord)
            {
                return;
            }

            var feed = await _feedRepository.FindById(notification.Feed.Id);
            if (feed == null)
            {
                throw new InvalidOperationException(
                    $"Feed id '{notification.Feed.Id}' does not exist.");
            }

            var details 
                = await _textChannelRepository
                    .FindByFeedId(notification.Feed.Id);

            if (details == null)
            {
                throw new InvalidOperationException(
                    $"No corresponding feed details for feed '{notification.Feed.Id}'.");
            }

            var guild = _client.GetGuild(details.Guild);
            if (guild == null)
            {
                throw new InvalidOperationException(
                    $"Server '{details.Guild}' does not exist on this bot instance.");
            }

            var channel = guild.GetChannel(details.Channel) as ITextChannel;
            if (channel == null)
            {
                throw new InvalidOperationException(
                    $"Text channel '{details.Channel}' does not exist on this server.");
            }

            var formatter = new StringFormatter();
            formatter.Add("@link", notification.Channel.StreamLink);

            var template = feed.MessageTemplate ?? DEFAULT_TEMPLATE;
            await channel.SendMessageAsync(formatter.Format(template));
        }
    }
}
