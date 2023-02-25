using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using Enna.Streamers.Domain.SeedWork;

namespace Enna.Streamers.Application.Workers
{
    public class FindLiveStreamersWorker : IWorker
    {
        private readonly IStreamerRepository _streamerRepository;
        private readonly IEnumerable<ILinkFetcher> _fetchers;
        private readonly IDomainEventDispatcher _dispatcher;

        public FindLiveStreamersWorker(
            IEnumerable<ILinkFetcher> fetchers,
            IStreamerRepository streamerRepository,
            IDomainEventDispatcher dispatcher)
        {
            ArgumentNullException.ThrowIfNull(streamerRepository);
            ArgumentNullException.ThrowIfNull(dispatcher);

            _streamerRepository = streamerRepository;
            _fetchers = fetchers;
            _dispatcher = dispatcher;
        }

        public async Task DoWork()
        {
            foreach (var streamer in await _streamerRepository.FindAll())
            {
                foreach (var channel in streamer.Channels)
                {
                    if (!streamer.Feeds.Any())
                    {
                        continue;
                    }

                    var foundFetcher = _fetchers.FirstOrDefault(
                        fetcher => fetcher.CanFetch(channel));

                    if (foundFetcher == null)
                    {
                        continue;
                    }

                    var streamLink = await foundFetcher.Fetch(channel);
                    if (streamLink == null)
                    {
                        channel.GoOffline();
                        continue;
                    }

                    channel.GoLive(streamLink);
                }
            }

            await _dispatcher.DispatchEventsAsync();
        }
    }
}
