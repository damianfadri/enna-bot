namespace Enna.Bot.SeedWork
{
    public class TenantEntity : Entity
    {
        public Guid? TenantId { get; set; }

        public TenantEntity(Guid id) : base(id)
        {
        }
    }
}
