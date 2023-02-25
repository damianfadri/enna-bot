using MediatR;

namespace Enna.Streamers.Application.Contracts
{
    public record AddTextChannelFeedRequest(
        Guid Id,
        ulong GuildId,
        ulong ChannelId)
        : IRequest;
}
