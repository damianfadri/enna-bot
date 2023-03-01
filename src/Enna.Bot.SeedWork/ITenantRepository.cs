namespace Enna.Bot.SeedWork
{
    public interface ITenantRepository<TKey> 
        : IRepository<Tenant<TKey>> where TKey : struct
    {
        Task<Tenant<TKey>?> FindByKey(TKey key);
    }
}
