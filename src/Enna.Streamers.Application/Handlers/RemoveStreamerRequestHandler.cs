using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using MediatR;

namespace Enna.Streamers.Application.Handlers
{
    public class RemoveStreamerRequestHandler
        : IRequestHandler<RemoveStreamerRequest>
    {
        private readonly IStreamerRepository _streamerRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IFeedRepository _feedRepository;

        public RemoveStreamerRequestHandler(
            IStreamerRepository streamerRepository,
            IChannelRepository channelRepository,
            IFeedRepository feedRepository) 
        {
            ArgumentNullException.ThrowIfNull(streamerRepository);
            ArgumentNullException.ThrowIfNull(channelRepository);
            ArgumentNullException.ThrowIfNull(feedRepository);

            _streamerRepository = streamerRepository;
            _channelRepository = channelRepository;
            _feedRepository = feedRepository;
        }

        public async Task Handle(
            RemoveStreamerRequest request, 
            CancellationToken cancellationToken)
        {
            var streamer = await _streamerRepository
                .FindById(request.StreamerId);

            if (streamer == null)
            {
                throw new InvalidOperationException(
                    $"Streamer id '{request.StreamerId}' does not exist.");
            }

            await _channelRepository.Remove(streamer.Channel);
            await _feedRepository.Remove(streamer.Feed);
            await _streamerRepository.Remove(streamer);
        }
    }
}
