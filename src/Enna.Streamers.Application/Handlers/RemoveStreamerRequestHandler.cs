using Enna.Core.Domain;
using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using MediatR;

namespace Enna.Streamers.Application.Handlers
{
    public class RemoveStreamerRequestHandler
        : IRequestHandler<RemoveStreamerRequest>
    {
        private readonly IStreamerRepository _streamerRepository;

        public RemoveStreamerRequestHandler(
            IStreamerRepository streamerRepository) 
        {
            ArgumentNullException.ThrowIfNull(streamerRepository);

            _streamerRepository = streamerRepository;
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

            await _streamerRepository.Remove(streamer);
        }
    }
}
