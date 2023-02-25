using Enna.Streamers.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Enna.Streamers.Infrastructure.Mssql
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly IDomainEventDispatcher _dispatcher;
        private readonly StreamerContext _context;

        public UnitOfWork(
            IDomainEventDispatcher dispatcher,
            StreamerContext context)
        {
            ArgumentNullException.ThrowIfNull(dispatcher);
            ArgumentNullException.ThrowIfNull(context);

            _dispatcher = dispatcher;
            _context = context;
        }

        public async Task CommitAsync()
        {
            await _dispatcher.DispatchEventsAsync();
            await _context.SaveChangesAsync();
        }
    }
}