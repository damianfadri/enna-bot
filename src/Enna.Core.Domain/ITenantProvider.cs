namespace Enna.Core.Domain
{
    public interface ITenantProvider
    {
        Guid TenantId { get; set; }
    }
}
