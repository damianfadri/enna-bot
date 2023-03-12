using Enna.Discord.Application.Contracts;
using Enna.Discord.Domain;
using Enna.Streamers.Domain;
using MediatR;

namespace Enna.Discord.Application.Handlers
{
    public class AddTextChannelFeedRequestHandler
        : IRequestHandler<AddTextChannelFeedRequest>
    {
        private readonly IFeedRepository _feedRepository;
        private readonly ITextChannelFeedRepository _textChannelRepository;

        public AddTextChannelFeedRequestHandler(
            IFeedRepository feedRepository,
            ITextChannelFeedRepository textChannelRepository)
        {
            ArgumentNullException.ThrowIfNull(feedRepository);
            ArgumentNullException.ThrowIfNull(textChannelRepository);

            _feedRepository = feedRepository;
            _textChannelRepository = textChannelRepository;
        }

        public async Task Handle(
            AddTextChannelFeedRequest request,
            CancellationToken cancellationToken)
        {
            var feed = await _feedRepository.FindById(request.FeedId);
            if (feed == null)
            {
                throw new InvalidOperationException(
                    $"Feed id '{request.FeedId}' does not exist.");
            }

            var textChannelFeed = 
                new TextChannelFeed(
                    request.Id,
                    request.GuildId,
                    request.ChannelId);

            await _textChannelRepository.Add(textChannelFeed);
        }
    }
}
