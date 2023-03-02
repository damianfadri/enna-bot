using Enna.Core.Domain;

namespace Enna.Bot.Infrastructure.Mssql
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly IDomainEventDispatcher _dispatcher;
        private readonly ITenantAssigner _assigner;
        private readonly StreamerContext _context;

        public UnitOfWork(
            IDomainEventDispatcher dispatcher,
            ITenantAssigner assigner,
            StreamerContext context)
        {
            ArgumentNullException.ThrowIfNull(dispatcher);
            ArgumentNullException.ThrowIfNull(assigner);
            ArgumentNullException.ThrowIfNull(context);

            _dispatcher = dispatcher;
            _assigner = assigner;
            _context = context;
        }

        public async Task CommitAsync()
        {
            await _dispatcher.DispatchEventsAsync();
            await _assigner.AssignAsync();
            await _context.SaveChangesAsync();
        }
    }
}