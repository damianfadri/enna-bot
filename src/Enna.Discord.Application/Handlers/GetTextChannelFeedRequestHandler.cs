using Enna.Discord.Application.Contracts;
using Enna.Discord.Domain;
using MediatR;

namespace Enna.Discord.Application.Handlers
{
    public class GetTextChannelFeedRequestHandler
        : IRequestHandler<GetTextChannelFeedRequest, TextChannelFeedDto>
    {
        private readonly ITextChannelFeedRepository _feedRepository;

        public GetTextChannelFeedRequestHandler(
            ITextChannelFeedRepository feedRepository) 
        {
            ArgumentNullException.ThrowIfNull(feedRepository);

            _feedRepository = feedRepository;
        }

        public async Task<TextChannelFeedDto> Handle(
            GetTextChannelFeedRequest request, 
            CancellationToken cancellationToken)
        {
            var feed = await _feedRepository.FindById(request.FeedId);

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
