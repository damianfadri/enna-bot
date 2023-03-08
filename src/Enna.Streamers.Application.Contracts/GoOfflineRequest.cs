using MediatR;

namespace Enna.Streamers.Application.Contracts
{
    public record GoOfflineRequest(
        Guid StreamerId,
        Guid ChannelId)
        : IRequest;
}
