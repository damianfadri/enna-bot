namespace Enna.Discord.Application.Contracts
{
    public record GuildTenantDto(
        Guid Id,
        ulong GuildId);
}