using MediatR;

namespace Enna.Streamers.Application.Contracts
{
    public record RemoveStreamerRequest(
        Guid StreamerId) 
        : IRequest;
}
