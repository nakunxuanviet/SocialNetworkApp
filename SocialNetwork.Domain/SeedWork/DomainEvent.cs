using MediatR;
using System;

namespace SocialNetwork.Domain.SeedWork
{
    public abstract class DomainEvent : INotification
    {
        public DomainEvent()
        {
            EventId = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public virtual Guid EventId { get; init; }

        public virtual DateTime CreatedAt { get; init; }
    }
}
