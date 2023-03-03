using Enna.Core.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Enna.Bot.Infrastructure.Mssql
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDomainEventDispatcher _dispatcher;
        private readonly ITenantAssigner _assigner;
        private readonly StreamerContext _streamerContext;
        private readonly TenantContext _tenantContext;

        public UnitOfWork(
            IDomainEventDispatcher dispatcher,
            ITenantAssigner assigner,
            StreamerContext streamerContext,
            TenantContext tenantContext)
        {
            ArgumentNullException.ThrowIfNull(dispatcher);
            ArgumentNullException.ThrowIfNull(assigner);
            ArgumentNullException.ThrowIfNull(streamerContext);
            ArgumentNullException.ThrowIfNull(tenantContext);

            _dispatcher = dispatcher;
            _assigner = assigner;
            _streamerContext = streamerContext;
            _tenantContext = tenantContext;
        }

        public async Task CommitAsync()
        {
            await _dispatcher.DispatchEventsAsync();
            await _assigner.AssignAsync();
            await _tenantContext.SaveChangesAsync();
            await _streamerContext.SaveChangesAsync();
        }
    }
}