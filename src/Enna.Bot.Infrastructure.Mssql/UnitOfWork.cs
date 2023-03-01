using Enna.Bot.SeedWork;

namespace Enna.Bot.Infrastructure.Mssql
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly IDomainEventDispatcher _dispatcher;
        private readonly ITenantAppender _appender;
        private readonly StreamerContext _context;

        public UnitOfWork(
            IDomainEventDispatcher dispatcher,
            ITenantAppender appender,
            StreamerContext context)
        {
            ArgumentNullException.ThrowIfNull(dispatcher);
            ArgumentNullException.ThrowIfNull(appender);
            ArgumentNullException.ThrowIfNull(context);

            _dispatcher = dispatcher;
            _appender = appender;
            _context = context;
        }

        public async Task CommitAsync()
        {
            await _dispatcher.DispatchEventsAsync();
            await _appender.AppendAsync();
            await _context.SaveChangesAsync();
        }
    }
}