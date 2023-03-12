using MediatR;

namespace Enna.Discord.Application.Contracts
{
    public record AddTextChannelFeedRequest(
        Guid Id,
        Guid FeedId,
        ulong GuildId,
        ulong ChannelId)
        : IRequest;
}
