using MediatR;

namespace Enna.Streamers.Application.Contracts
{
    public record GoLiveRequest(
        Guid StreamerId, 
        Guid ChannelId,
        string StreamLink)
        : IRequest;
}
