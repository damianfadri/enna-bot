using Enna.Core.Domain;
using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using MediatR;

namespace Enna.Bot.Workers
{
    public class FindLiveStreamersWorker : TenantBaseWorker
    {
        private readonly IEnumerable<ILinkFetcher> _fetchers;

        public FindLiveStreamersWorker(
            IEnumerable<ILinkFetcher> fetchers,
            IMediator mediator,
            IUnitOfWork unitOfWork)
            : base(mediator, unitOfWork)
        {
            _fetchers = fetchers;
        }

        public override async Task DoWork(params object[] args)
        {
            await base.DoWork(args);

            var streamers = 
                await SendToTenantAsync(
                    new ListStreamersRequest());

            foreach (var streamer in streamers)
            {
                foreach (var channel in streamer.Channels)
                {
                    if (!streamer.Feeds.Any())
                    {
                        continue;
                    }

                    var foundFetcher = _fetchers.FirstOrDefault(
                        fetcher => fetcher.CanFetch(channel.Link));

                    if (foundFetcher == null)
                    {
                        continue;
                    }

                    var streamLink = await foundFetcher.Fetch(channel.Link);
                    if (streamLink == null)
                    {
                        await SendToTenantAsync(
                            new GoOfflineRequest(
                                streamer.Id,
                                channel.Id));

                        continue;
                    }

                    await SendToTenantAsync(
                        new GoLiveRequest(
                            streamer.Id, 
                            channel.Id, 
                            streamLink));
                }
            }

            await UnitOfWork.CommitAsync();
        }
    }
}
