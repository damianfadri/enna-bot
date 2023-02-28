using MediatR;

namespace Enna.Discord.Application.Contracts
{
    public record GetTextChannelFeedRequest(
        Guid FeedId)
        : IRequest<TextChannelFeedDto>;
}
