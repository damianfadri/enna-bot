using Enna.Core.Domain;
using Enna.Discord.Application.Contracts;
using Enna.Discord.Domain;
using Enna.Streamers.Domain;
using MediatR;

namespace Enna.Discord.Application.Handlers
{
    public class AddTextChannelFeedRequestHandler
        : IRequestHandler<AddTextChannelFeedRequest>
    {
        private readonly IFeedRepository _feedRepository;
        private readonly ITextChannelFeedRepository _textChannelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddTextChannelFeedRequestHandler(
            IFeedRepository feedRepository,
            ITextChannelFeedRepository textChannelRepository,
            IUnitOfWork unitOfWork)
        {
            ArgumentNullException.ThrowIfNull(feedRepository);
            ArgumentNullException.ThrowIfNull(textChannelRepository);
            ArgumentNullException.ThrowIfNull(unitOfWork);

            _feedRepository = feedRepository;
            _textChannelRepository = textChannelRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            AddTextChannelFeedRequest request,
            CancellationToken cancellationToken)
        {
            var feed = await _feedRepository.FindById(request.Id);
            if (feed == null)
            {
                throw new InvalidOperationException(
                    $"Feed id '{request.Id}' does not exist.");
            }

            if (feed.Type != FeedType.Discord)
            {
                throw new InvalidOperationException(
                    $"Feed id '{request.Id}' is not a '{FeedType.Discord}' feed type.");
            }

            var textChannelFeed = new TextChannelFeed(
                request.Id,
                request.GuildId,
                request.ChannelId);

            await _textChannelRepository.Add(textChannelFeed);
            await _unitOfWork.CommitAsync();
        }
    }
}
