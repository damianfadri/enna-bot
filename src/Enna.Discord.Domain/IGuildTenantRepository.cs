using Enna.Core.Domain;
using Enna.Streamers.Domain;

namespace Enna.Discord.Domain
{
    public interface IGuildTenantRepository : IRepository<Tenant<ulong>>
    {
        Task<Tenant<ulong>?> FindByGuildId(ulong guildId);
    }
}
