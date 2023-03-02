using Enna.Bot.SeedWork;

namespace Enna.Bot.Infrastructure.Tenant
{
    public class TenantContext : ITenantContext
    {
        public Guid TenantId { get; set; }
    }
}
