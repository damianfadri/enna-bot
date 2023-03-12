using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using MediatR;

namespace Enna.Streamers.Application.Handlers
{
    public class GoLiveRequestHandler : IRequestHandler<GoLiveRequest>
    {
        private IStreamerRepository _streamerRepository;

        public GoLiveRequestHandler(
            IStreamerRepository streamerRepository) 
        {
            ArgumentNullException.ThrowIfNull(streamerRepository);

            _streamerRepository = streamerRepository;
        }

        public async Task Handle(
            GoLiveRequest request, 
            CancellationToken cancellationToken)
        {
            var streamer = await _streamerRepository
                .FindById(request.StreamerId);

            if (streamer == null)
            {
                throw new InvalidOperationException(
                    $"Streamer id {request.StreamerId} does not exist.");
            }

            streamer.GoLive(request.StreamLink);
        }
    }
}
