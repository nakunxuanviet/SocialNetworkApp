﻿using NaKun.Arc.Domain.Events;
using SocialNetwork.Domain.Entities.TodoItems;
using SocialNetwork.Domain.SeedWork;

namespace SocialNetwork.Domain.Entities.TodoItems.Events
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