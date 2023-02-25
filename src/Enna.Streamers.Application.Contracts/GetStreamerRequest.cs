using MediatR;

namespace Enna.Streamers.Application.Contracts
{
    public record GetStreamerRequest(
        Guid StreamerId)
        : IRequest<StreamerDto>;
}
