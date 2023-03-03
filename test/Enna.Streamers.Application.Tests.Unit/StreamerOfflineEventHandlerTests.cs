using Enna.Streamers.Domain;
using Enna.Streamers.Domain.Events;
using Xunit;

namespace Enna.Streamers.Application.Tests.Unit
{
    public class StreamerOfflineEventHandlerTests
    {
        public class Handle_Should 
        {
            [Fact]
            public async Task DoNothing()
            {
                var handler =
                    new StreamerOfflineEventHandlerSutBuilder()
                        .Build();

                var streamer = new Streamer(
                    Guid.NewGuid(),
                    "Streamer name");

                var channel = new Channel(
                    Guid.NewGuid(),
                    "https://youtube.com/channel-link");

                channel.GoLive("https://youtube.com/stream-link");
                channel.GoOffline();

                await handler.Handle(
                    new StreamerOfflineEvent(
                        streamer,
                        channel),
                    CancellationToken.None);
            }
        }
    }
}
