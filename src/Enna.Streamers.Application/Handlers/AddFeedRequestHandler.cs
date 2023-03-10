using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using MediatR;

namespace Enna.Streamers.Application.Handlers
{
    public class AddFeedRequestHandler : IRequestHandler<AddFeedRequest>
    {
        private readonly IStreamerRepository _streamerRepository;
        private readonly IFeedRepository _feedRepository;

        public AddFeedRequestHandler(
            IStreamerRepository streamerRepository,
            IFeedRepository feedRepository)
        {
            ArgumentNullException.ThrowIfNull(streamerRepository);
            ArgumentNullException.ThrowIfNull(feedRepository);

            _streamerRepository = streamerRepository;
            _feedRepository = feedRepository;
        }

        public async Task Handle(
            AddFeedRequest request, 
            CancellationToken cancellationToken)
        {
            var streamer = await _streamerRepository
                .FindById(request.StreamerId);

            if (streamer == null)
            {
                throw new InvalidOperationException(
                    $"Streamer id '{request.StreamerId}' does not exist.");
            }

            if (!Enum.TryParse(
                value: request.FeedType,
                ignoreCase: true,
                result: out FeedType feedType))
            {
                throw new InvalidOperationException(
                    $"Feed type '{request.FeedType}' is unrecognized.");
            }


            if (string.IsNullOrWhiteSpace(request.MessageTemplate)
                || !request.MessageTemplate.Contains("@link"))
            {
                throw new InvalidOperationException(
                    $"Template message should contain an @link.");
            }

            var feed = new Feed(
                request.Id, 
                feedType, 
                request.MessageTemplate);

            streamer.Feeds.Add(feed);

            await _feedRepository.Add(feed);
        }
    }
}
