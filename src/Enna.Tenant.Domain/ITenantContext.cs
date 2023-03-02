namespace Enna.Tenant.Domain
{
    public interface ITenantContext
    {
        Guid TenantId { get; set; }
    }
}
