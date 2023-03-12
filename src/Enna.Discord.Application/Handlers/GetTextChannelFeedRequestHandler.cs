using Enna.Discord.Application.Contracts;
using Enna.Discord.Domain;
using MediatR;

namespace Enna.Discord.Application.Handlers
{
    public class GetTextChannelFeedRequestHandler
        : IRequestHandler<GetTextChannelFeedRequest, TextChannelFeedDto>
    {
        private readonly ITextChannelFeedRepository _textChannelRepository;

        public GetTextChannelFeedRequestHandler(
            ITextChannelFeedRepository textChannelRepository) 
        {
            ArgumentNullException.ThrowIfNull(textChannelRepository);

            _textChannelRepository = textChannelRepository;
        }

        public async Task<TextChannelFeedDto> Handle(
            GetTextChannelFeedRequest request, 
            CancellationToken cancellationToken)
        {
            var feed = await _textChannelRepository.FindByFeedId(request.FeedId);

            if (feed == null)
            {
                throw new InvalidOperationException(
                    $"Feed id {request.FeedId} does not exist.");
            }

            return new TextChannelFeedDto(
                feed.Id,
                feed.Guild,
                feed.Channel);
        }
    }
}
