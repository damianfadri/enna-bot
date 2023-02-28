namespace Enna.Discord.Application.Contracts
{
    public record TextChannelFeedDto(
        Guid Id,
        ulong GuildId,
        ulong ChannelId);
}