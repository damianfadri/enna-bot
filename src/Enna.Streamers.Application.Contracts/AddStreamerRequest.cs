using MediatR;

namespace Enna.Streamers.Application.Contracts
{
    public record AddStreamerRequest(
        Guid Id,
        string Name,
        string ChannelLink)
        : IRequest;
}
