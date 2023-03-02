namespace Enna.Tenant.Domain
{
    public interface ITenantAppender
    {
        Task AppendAsync();
    }
}
