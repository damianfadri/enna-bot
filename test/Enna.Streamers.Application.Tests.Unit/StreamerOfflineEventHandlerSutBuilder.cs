using Enna.Streamers.Application.Handlers;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class StreamerOfflineEventHandlerSutBuilder
    {
        public StreamerOfflineEventHandlerSutBuilder()
        {
        }

        public StreamerOfflineEventHandler Build()
        {
            return new StreamerOfflineEventHandler();
        }
    }
}
