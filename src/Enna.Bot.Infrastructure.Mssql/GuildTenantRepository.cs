using Enna.Discord.Domain;
using Enna.Streamers.Domain;
using Microsoft.EntityFrameworkCore;

namespace Enna.Bot.Infrastructure.Mssql
{
    public class GuildTenantRepository : IGuildTenantRepository
    {
        private readonly TenantContext _context;

        public GuildTenantRepository(TenantContext context) 
        {
            ArgumentNullException.ThrowIfNull(context);

            _context = context;
        }

        public async Task Add(Tenant<ulong> entity)
        {
            await _context.Tenants.AddAsync(entity);
        }

        public async Task<IEnumerable<Tenant<ulong>>> FindAll()
        {
            return await _context.Tenants.ToListAsync();
        }

        public async Task<Tenant<ulong>?> FindById(Guid id)
        {
            return await _context.Tenants
                .FirstOrDefaultAsync(
                    tenant => tenant.Id == id);
        }

        public async Task<Tenant<ulong>?> FindByGuildId(ulong guildId)
        {
            return await _context.Tenants
                .FirstOrDefaultAsync(
                    tenant => tenant.KeyId == guildId);
        }

        public async Task Remove(Tenant<ulong> entity)
        {
            _context.Tenants.Remove(entity);
            await Task.CompletedTask;
        }
    }
}
