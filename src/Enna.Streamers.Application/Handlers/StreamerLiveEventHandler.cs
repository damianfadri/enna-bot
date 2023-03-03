using Enna.Streamers.Domain.Events;
using MediatR;

namespace Enna.Streamers.Application.Handlers
{
    public class StreamerLiveEventHandler 
        : INotificationHandler<StreamerLiveEvent>
    {
        public StreamerLiveEventHandler()
        {
        }

        public async Task Handle(
            StreamerLiveEvent notification, 
            CancellationToken cancellationToken)
        {
            foreach (var feed in notification.Streamer.Feeds)
            {
                feed.Notify(notification.Channel);
            }

            await Task.CompletedTask;
        }
    }
}
