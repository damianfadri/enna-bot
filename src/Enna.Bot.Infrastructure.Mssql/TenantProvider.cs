using Enna.Bot.SeedWork;

namespace Enna.Bot.Infrastructure.Mssql
{
    public class TenantProvider : ITenantProvider
    {
        public Guid TenantId { get; set; }
        public TenantProvider()
        {

        }
    }
}
