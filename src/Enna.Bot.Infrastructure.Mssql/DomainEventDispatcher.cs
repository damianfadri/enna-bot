using Enna.Core.Domain;
using MediatR;

namespace Enna.Bot.Infrastructure.Mssql
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly StreamerContext _context;
        private readonly IMediator _mediator;

        public DomainEventDispatcher(
            StreamerContext context,
            IMediator mediator)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(mediator);

            _context = context;
            _mediator = mediator;
        }

        public async Task DispatchEventsAsync()
        {
            var changedEntities
                = _context
                    .ChangeTracker
                    .Entries<Entity>()
                    .Select(entry => entry.Entity)
                    .ToList();

            var pendingEvents
                = changedEntities
                    .SelectMany(entity => entity.GetEvents())
                    .ToList();

            foreach (var entity in changedEntities)
            {
                entity.ClearEvents();
            }

            var tasks
                = pendingEvents
                    .Select(async (pendingEvent) => await _mediator.Publish(pendingEvent));

            await Task.WhenAll(tasks);
        }
    }
}
