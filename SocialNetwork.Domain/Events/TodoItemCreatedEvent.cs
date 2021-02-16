using SocialNetwork.Domain.Entities;
using SocialNetwork.Domain.SeedWork;

namespace SocialNetwork.Domain.Events
{
    public class TodoItemCreatedEvent : DomainEvent
    {
        public TodoItemCreatedEvent(TodoItem item)
        {
            Item = item;
        }

        public TodoItem Item { get; }
    }
}