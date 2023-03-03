namespace Enna.Core.Domain
{
    public class TenantEntity : Entity
    {
        public Guid? TenantId { get; set; }

        public TenantEntity(Guid id) : base(id)
        {
        }
    }
}
