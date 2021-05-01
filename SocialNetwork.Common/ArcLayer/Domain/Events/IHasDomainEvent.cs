using System.Collections.Generic;

namespace SocialNetwork.Common.ArcLayer.Domain.Events
{
    public interface IHasDomainEvent
    {
        public List<DomainEvent> DomainEvents { get; set; }
    }
}