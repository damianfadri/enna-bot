using Enna.Core.Domain;
using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using MediatR;

namespace Enna.Streamers.Application.Handlers
{
    public class AddFeedRequestHandler : IRequestHandler<AddFeedRequest>
    {
        private readonly IStreamerRepository _streamerRepository;
        private readonly IFeedRepository _feedRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddFeedRequestHandler(
            IStreamerRepository streamerRepository,
            IFeedRepository feedRepository,
            IUnitOfWork unitOfWork)
        {
            ArgumentNullException.ThrowIfNull(streamerRepository);
            ArgumentNullException.ThrowIfNull(feedRepository);
            ArgumentNullException.ThrowIfNull(unitOfWork);

            _streamerRepository = streamerRepository;
            _feedRepository = feedRepository;
            _unitOfWork = unitOfWork;
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

            var feed = new Feed(request.Id, feedType);
            streamer.Feeds.Add(feed);

            await _feedRepository.Add(feed);
            await _unitOfWork.CommitAsync();
        }
    }
}
