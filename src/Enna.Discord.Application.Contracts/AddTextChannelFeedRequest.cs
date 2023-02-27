using MediatR;

namespace Enna.Discord.Application.Contracts
{
    public record AddTextChannelFeedRequest(
        Guid Id,
        ulong GuildId,
        ulong ChannelId)
        : IRequest;
}
