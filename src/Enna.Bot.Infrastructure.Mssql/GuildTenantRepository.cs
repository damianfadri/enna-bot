using Enna.Bot.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Enna.Bot.Infrastructure.Mssql
{
    public class GuildTenantRepository : ITenantRepository<ulong>
    {
        private readonly StreamerContext _context;

        public GuildTenantRepository(StreamerContext context) 
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

        public async Task<Tenant<ulong>?> FindByKey(ulong key)
        {
            return await _context.Tenants
                .FirstOrDefaultAsync(
                    tenant => tenant.KeyId == key);
        }

        public async Task Remove(Tenant<ulong> entity)
        {
            _context.Tenants.Remove(entity);
            await Task.CompletedTask;
        }
    }
}
