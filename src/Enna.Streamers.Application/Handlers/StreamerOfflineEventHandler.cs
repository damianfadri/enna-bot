using Enna.Streamers.Domain.Events;
using Enna.Streamers.Domain.SeedWork;
using MediatR;

namespace Enna.Streamers.Application.Handlers
{
    public class StreamerOfflineEventHandler 
        : INotificationHandler<StreamerOfflineEvent>
    {
        private readonly IUnitOfWork _unitOfWork;

        public StreamerOfflineEventHandler(IUnitOfWork unitOfWork)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork);

            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            StreamerOfflineEvent notification, 
            CancellationToken cancellationToken)
        {
            await _unitOfWork.CommitAsync();
        }
    }
}
