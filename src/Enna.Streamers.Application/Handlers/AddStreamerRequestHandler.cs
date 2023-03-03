using Enna.Core.Domain;
using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using MediatR;

namespace Enna.Streamers.Application.Handlers
{
    public class AddStreamerRequestHandler 
        : IRequestHandler<AddStreamerRequest>
    {
        private readonly IStreamerRepository _streamerRepository;
        private readonly IChannelRepository _channelRepository;

        public AddStreamerRequestHandler(
            IStreamerRepository streamerRepository,
            IChannelRepository channelRepository)
        {
            ArgumentNullException.ThrowIfNull(streamerRepository);
            ArgumentNullException.ThrowIfNull(channelRepository);

            _streamerRepository = streamerRepository;
            _channelRepository = channelRepository;
        }

        public async Task Handle(
            AddStreamerRequest request, 
            CancellationToken cancellationToken)
        {
            var streamer = new Streamer(request.Id, request.Name);
            var channel = new Channel(Guid.NewGuid(), request.ChannelLink);

            streamer.Channels.Add(channel);

            await _streamerRepository.Add(streamer);
            await _channelRepository.Add(channel);
        }
    }
}