using Enna.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Enna.Bot.Infrastructure.Mssql
{
    public class TenantAssigner : ITenantAssigner
    {
        private readonly ITenantProvider _tenantProvider;
        private readonly StreamerContext _context;

        public TenantAssigner(
            ITenantProvider tenantProvider,
            StreamerContext context)
        {
            ArgumentNullException.ThrowIfNull(tenantProvider);
            ArgumentNullException.ThrowIfNull(context);

            _tenantProvider = tenantProvider;
            _context = context;
        }

        public async Task AssignAsync()
        {
            var addedEntities
                = _context
                    .ChangeTracker
                    .Entries<TenantEntity>()
                    .Where(entry => entry.State == EntityState.Added)
                    .Select(entry => entry.Entity)
                    .ToList();

            foreach (var addedEntity in addedEntities)
            {
                addedEntity.TenantId = _tenantProvider.TenantId;
            }

            await Task.CompletedTask;
        }
    }
}
