using MediatR;

namespace Enna.Discord.Application.Contracts
{
    public record AddGuildTenantRequest(
        Guid Id,
        ulong GuildId)
        : IRequest;
}
