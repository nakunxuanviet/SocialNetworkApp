using SocialNetwork.Domain.Entities;
using SocialNetwork.Domain.SeedWork;

namespace SocialNetwork.Domain.Events
{
    public class TodoItemCompletedEvent : DomainEvent
    {
        public TodoItemCompletedEvent(TodoItem item)
        {
            Item = item;
        }

        public TodoItem Item { get; }
    }
}