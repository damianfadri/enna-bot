using Enna.Bot.SeedWork;

namespace Enna.Discord.Tenant
{
    public class GuildTenantRepository : ITenantRepository<ulong>
    {
        public Guid FindByKey(ulong foreignKey)
        {
            throw new NotImplementedException();
        }
    }
}