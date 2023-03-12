using Enna.Discord.Application.Contracts;
using Enna.Discord.Domain;
using MediatR;

namespace Enna.Discord.Application.Handlers
{
    public class RemoveTextChannelFeedRequestHandler : 
        IRequestHandler<RemoveTextChannelFeedRequest>
    {
        private readonly ITextChannelFeedRepository _textChannelRepository;

        public RemoveTextChannelFeedRequestHandler(
            ITextChannelFeedRepository textChannelRepository)
        {
            ArgumentNullException.ThrowIfNull(textChannelRepository);

            _textChannelRepository = textChannelRepository;
        }

        public async Task Handle(
            RemoveTextChannelFeedRequest request, 
            CancellationToken cancellationToken)
        {
            var feed = await _textChannelRepository.FindByFeedId(request.FeedId);

            if (feed == null)
            {
                throw new InvalidOperationException(
                    $"Feed id {request.FeedId} does not exist.");
            }

            await _textChannelRepository.Remove(feed);
        }
    }
}
