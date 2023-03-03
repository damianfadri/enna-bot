using MediatR;

namespace Enna.Discord.Application.Contracts
{
    public record GetGuildTenantByGuildRequest(
        ulong GuildId) 
        : IRequest<GuildTenantDto>;
}
