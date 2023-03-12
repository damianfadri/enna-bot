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

            streamer.GoOffline();
        }
    }
}
