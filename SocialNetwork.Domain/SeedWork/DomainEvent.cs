using MediatR;
using System;
using System.Collections.Generic;

namespace SocialNetwork.Domain.SeedWork
{
    public interface IHasDomainEvent
    {
        public List<DomainEvent> DomainEvents { get; set; }
    }

    public abstract class DomainEvent : INotification
    {
        public DomainEvent()
        {
            EventId = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public virtual Guid EventId { get; init; }
        public bool IsPublished { get; set; }
        public virtual DateTime CreatedAt { get; init; }
    }
}
