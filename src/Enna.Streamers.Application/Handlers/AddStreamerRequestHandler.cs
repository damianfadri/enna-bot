using Enna.Streamers.Application.Contracts;
using Enna.Streamers.Domain;
using Enna.Streamers.Domain.SeedWork;
using MediatR;

namespace Enna.Streamers.Application.Handlers
{
    public class AddStreamerRequestHandler 
        : IRequestHandler<AddStreamerRequest>
    {
        private readonly IStreamerRepository _streamerRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddStreamerRequestHandler(
            IStreamerRepository streamerRepository,
            IChannelRepository channelRepository,
            IUnitOfWork unitOfWork)
        {
            ArgumentNullException.ThrowIfNull(streamerRepository);
            ArgumentNullException.ThrowIfNull(channelRepository);
            ArgumentNullException.ThrowIfNull(unitOfWork);

            _streamerRepository = streamerRepository;
            _channelRepository = channelRepository;
            _unitOfWork = unitOfWork;
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

            await _unitOfWork.CommitAsync();
        }
    }
}