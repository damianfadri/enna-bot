using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using Enna.Streamers.Domain.SeedWork;
using MediatR;

namespace Enna.Streamers.Application.Handlers
{
    public class RemoveStreamerRequestHandler
        : IRequestHandler<RemoveStreamerRequest>
    {
        private readonly IStreamerRepository _streamerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveStreamerRequestHandler(
            IStreamerRepository streamerRepository,
            IUnitOfWork unitOfWork) 
        {
            ArgumentNullException.ThrowIfNull(streamerRepository);
            ArgumentNullException.ThrowIfNull(unitOfWork);

            _streamerRepository = streamerRepository;
            _unitOfWork = unitOfWork;
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
            await _unitOfWork.CommitAsync();
        }
    }
}
