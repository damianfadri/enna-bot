using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using MediatR;

namespace Enna.Streamers.Application.Handlers
{
    public class AddStreamerRequestHandler 
        : IRequestHandler<AddStreamerRequest>
    {
        private readonly IStreamerRepository _streamerRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IFeedRepository _feedRepository;
        private readonly IEnumerable<ILinkFetcher> _fetchers;

        public AddStreamerRequestHandler(
            IStreamerRepository streamerRepository,
            IChannelRepository channelRepository,
            IFeedRepository feedRepository,
            IEnumerable<ILinkFetcher> fetchers)
        {
            ArgumentNullException.ThrowIfNull(streamerRepository);
            ArgumentNullException.ThrowIfNull(channelRepository);
            ArgumentNullException.ThrowIfNull(feedRepository);

            _streamerRepository = streamerRepository;
            _channelRepository = channelRepository;
            _feedRepository = feedRepository;
            _fetchers = fetchers;
        }

        public async Task Handle(
            AddStreamerRequest request, 
            CancellationToken cancellationToken)
        {
            var foundFetcher = 
                _fetchers.FirstOrDefault(
                    fetcher => fetcher.CanFetch(request.ChannelLink));

            if (foundFetcher == null)
            {
                throw new InvalidOperationException(
                    $"'{request.ChannelLink}' is not a valid channel link.");
            }

            if (!Enum.TryParse<FeedType>(
                value: request.FeedType, 
                ignoreCase: true, 
                result: out var feedType))
            {
                throw new InvalidOperationException(
                    $"'{request.FeedType}' is not a valid feed type.");
            }

            var streamer 
                = new Streamer(
                    request.Id, 
                    request.Name,
                    new Channel(
                        Guid.NewGuid(), 
                        request.ChannelLink),
                    new Feed(
                        Guid.NewGuid(),
                        feedType,
                        request.MessageTemplate));

            await _feedRepository.Add(streamer.Feed);
            await _channelRepository.Add(streamer.Channel);
            await _streamerRepository.Add(streamer);
        }
    }
}