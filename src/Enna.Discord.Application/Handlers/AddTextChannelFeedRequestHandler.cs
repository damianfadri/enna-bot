using Enna.Core.Domain;
using Enna.Discord.Application.Contracts;
using Enna.Discord.Domain;
using Enna.Streamers.Domain;
using MediatR;

namespace Enna.Discord.Application.Handlers
{
    public class AddTextChannelFeedRequestHandler
        : IRequestHandler<AddTextChannelFeedRequest>
    {
        private readonly ITextChannelFeedRepository _textChannelRepository;

        public AddTextChannelFeedRequestHandler(
            ITextChannelFeedRepository textChannelRepository)
        {
            ArgumentNullException.ThrowIfNull(textChannelRepository);

            _textChannelRepository = textChannelRepository;
        }

        public async Task Handle(
            AddTextChannelFeedRequest request,
            CancellationToken cancellationToken)
        {
            var textChannelFeed = new TextChannelFeed(
                request.Id,
                request.GuildId,
                request.ChannelId);

            await _textChannelRepository.Add(textChannelFeed);
        }
    }
}
