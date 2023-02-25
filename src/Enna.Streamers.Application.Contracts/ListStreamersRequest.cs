using MediatR;

namespace Enna.Streamers.Application.Contracts
{
    public record ListStreamersRequest()
        : IRequest<IEnumerable<StreamerDto>>;
}
