namespace Enna.Streamers.Domain.SeedWork
{
    public class Entity
    {
        public Guid Id { get; set; }

        private List<IDomainEvent> _domainEvents;

        public Entity(Guid id)
        {
            _domainEvents = new List<IDomainEvent>();

            Id = id;
        }

        public void AddEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearEvents()
        {
            _domainEvents.Clear();
        }

        public IEnumerable<IDomainEvent> GetEvents()
        {
            foreach (var domainEvent in _domainEvents)
            {
                yield return domainEvent;
            }
        }
    }
}
