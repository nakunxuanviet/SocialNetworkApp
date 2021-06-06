using System.Collections.Generic;

namespace SocialNetwork.Domain.SeedWork
{
    public abstract class EntityBase
    {
        private List<DomainEvent> _events;
        public IReadOnlyList<DomainEvent> Events => _events.AsReadOnly();

        protected void AddEvent(DomainEvent @event)
        {
            _events.Add(@event);
        }

        protected void RemoveEvent(DomainEvent @event)
        {
            _events.Remove(@event);
        }
    }

    public abstract class EntityBase<TKey> : EntityBase
    {
        public TKey Id { get; set; }
    }
}
