using Enna.Core.Domain;

namespace Enna.Bot.Infrastructure.Mssql
{
    public class TenantProvider : ITenantProvider
    {
        public Guid TenantId { get; set; }
    }
}
