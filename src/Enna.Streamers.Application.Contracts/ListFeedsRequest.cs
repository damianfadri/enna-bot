using MediatR;

namespace Enna.Streamers.Application.Contracts
{
    public record ListFeedsRequest(
        Guid StreamerId)
        : IRequest<IEnumerable<FeedDto>>;
}
