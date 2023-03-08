using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using MediatR;

namespace Enna.Streamers.Application.Handlers
{
    public class GoOfflineRequestHandler : IRequestHandler<GoOfflineRequest>
    {
        private readonly IStreamerRepository _streamerRepository;

        public GoOfflineRequestHandler(IStreamerRepository streamerRepository) 
        {
            ArgumentNullException.ThrowIfNull(streamerRepository);

            _streamerRepository = streamerRepository;
        }

        public async Task Handle(
            GoOfflineRequest request, 
            CancellationToken cancellationToken)
        {
            var streamer = await _streamerRepository
                .FindById(request.StreamerId);

            if (streamer == null)
            {
                throw new InvalidOperationException(
                    $"Streamer id {request.StreamerId} does not exist.");
            }

            var channel = streamer.Channels
                .FirstOrDefault(channel => channel.Id == request.ChannelId);

            if (channel == null)
            {
                throw new InvalidOperationException(
                    $"Channel id {request.ChannelId} does not exist for streamer.");
            }

            streamer.GoOffline(channel);
        }
    }
}
