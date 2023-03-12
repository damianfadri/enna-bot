using MediatR;

namespace Enna.Streamers.Application.Contracts
{
    public record GoOfflineRequest(
        Guid StreamerId)
        : IRequest;
}
