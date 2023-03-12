using MediatR;

namespace Enna.Streamers.Application.Contracts
{
    public record GoLiveRequest(
        Guid StreamerId,
        string StreamLink)
        : IRequest;
}
