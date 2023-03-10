using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using MediatR;

namespace Enna.Streamers.Application.Handlers
{
    public class ListFeedsRequestHandler
        : IRequestHandler<ListFeedsRequest, IEnumerable<FeedDto>>
    {
        private const int DEFAULT_MAX_ITEMS = 5;

        private readonly IFeedRepository _feedRepository;

        public ListFeedsRequestHandler(
            IFeedRepository feedRepository) 
        {
            ArgumentNullException.ThrowIfNull(feedRepository);

            _feedRepository = feedRepository;
        }

        public async Task<IEnumerable<FeedDto>> Handle(
            ListFeedsRequest request, 
            CancellationToken cancellationToken)
        {
            var feeds = await _feedRepository
                .FindByStreamerId(request.StreamerId);

            return feeds
                .Take(DEFAULT_MAX_ITEMS)
                .Select(feed =>
                    new FeedDto(
                        feed.Id,
                        feed.Type.ToString(),
                        feed.MessageTemplate));
        }
    }
}
