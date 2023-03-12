using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using MediatR;

namespace Enna.Streamers.Application.Handlers
{
    public class GetStreamerRequestHandler 
        : IRequestHandler<GetStreamerRequest, StreamerDto>
    {
        private readonly IStreamerRepository _streamerRepository;

        public GetStreamerRequestHandler(
            IStreamerRepository streamerRepository)
        {
            ArgumentNullException.ThrowIfNull(streamerRepository);

            _streamerRepository = streamerRepository;
        }

        public async Task<StreamerDto> Handle(
            GetStreamerRequest request, 
            CancellationToken cancellationToken)
        {
            var streamer = await _streamerRepository
                .FindById(request.StreamerId);

            if (streamer == null)
            {
                throw new InvalidOperationException(
                    $"Streamer id {request.StreamerId} does not exist.");
            }

            return 
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
                        streamer.Feed.MessageTemplate));
        }
    }
}
