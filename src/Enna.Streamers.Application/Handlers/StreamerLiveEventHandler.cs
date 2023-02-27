using Enna.Bot.SeedWork;
using Enna.Streamers.Domain.Events;
using MediatR;

namespace Enna.Streamers.Application.Handlers
{
    public class StreamerLiveEventHandler 
        : INotificationHandler<StreamerLiveEvent>
    {
        private readonly IUnitOfWork _unitOfWork;

        public StreamerLiveEventHandler(IUnitOfWork unitOfWork)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork);

            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            StreamerLiveEvent notification, 
            CancellationToken cancellationToken)
        {
            foreach (var feed in notification.Streamer.Feeds)
            {
                feed.Notify(notification.Channel);
            }

            await _unitOfWork.CommitAsync();
        }
    }
}
