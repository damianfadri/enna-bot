using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using MediatR;

namespace Enna.Streamers.Application.Handlers
{
    public class ListStreamersRequestHandler 
        : IRequestHandler<ListStreamersRequest, IEnumerable<StreamerDto>>
    {
        private const int DEFAULT_MAX_ITEMS = 5;

        private readonly IStreamerRepository _streamerRepository;

        public ListStreamersRequestHandler(
            IStreamerRepository streamerRepository)
        {
            ArgumentNullException.ThrowIfNull(streamerRepository);

            _streamerRepository = streamerRepository;
        }

        public async Task<IEnumerable<StreamerDto>> Handle(
            ListStreamersRequest request,
            CancellationToken cancellationToken)
        {
            var streamers = await _streamerRepository.FindAll();

            return streamers
                .Take(DEFAULT_MAX_ITEMS)
                .Select(streamer =>
                    new StreamerDto(
                        streamer.Id,
                        streamer.Name,
                        new ChannelDto(
                            streamer.Channel.Id,
                            streamer.Channel.Link,
                            streamer.Channel.StreamLink),
                        new FeedDto(
                            streamer.Feed.Id,
                            streamer.Feed.Type.ToString(),
                            streamer.Feed.MessageTemplate)));
        }
    }
}