using Enna.Streamers.Domain.Events;
using MediatR;

namespace Enna.Streamers.Application.Handlers
{
    public class StreamerOfflineEventHandler 
        : INotificationHandler<StreamerOfflineEvent>
    {
        public StreamerOfflineEventHandler()
        {
        }

        public async Task Handle(
            StreamerOfflineEvent notification, 
            CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
