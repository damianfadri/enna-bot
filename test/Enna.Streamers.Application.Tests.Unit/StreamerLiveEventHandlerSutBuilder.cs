using Enna.Streamers.Application.Handlers;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class StreamerLiveEventHandlerSutBuilder
    {
        public StreamerLiveEventHandlerSutBuilder()
        {
        }

        public StreamerLiveEventHandler Build()
        {
            return new StreamerLiveEventHandler();
        }
    }
}
