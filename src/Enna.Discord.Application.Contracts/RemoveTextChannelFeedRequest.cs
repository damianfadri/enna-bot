using MediatR;

namespace Enna.Discord.Application.Contracts
{
    public record RemoveTextChannelFeedRequest(
        Guid FeedId)
        : IRequest;
}
