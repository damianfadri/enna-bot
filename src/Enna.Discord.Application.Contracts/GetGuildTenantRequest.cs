using MediatR;

namespace Enna.Discord.Application.Contracts
{
    public record GetGuildTenantRequest(
        ulong GuildId) 
        : IRequest<GuildTenantDto>;
}
