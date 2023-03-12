using MediatR;

namespace Enna.Streamers.Application.Contracts
{
    public record AddFeedRequest(
        Guid Id,
        Guid StreamerId,
        string FeedType,
        string? MessageTemplate = null)
        : IRequest;
}
