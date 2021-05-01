using NaKun.Arc.Domain.Events;
using NaKun.Arc.Domain.SeedWork;
using SocialNetwork.Domain.Entities.TodoItems.Events;
using SocialNetwork.Domain.SeedWork;
using System;
using System.Collections.Generic;

namespace SocialNetwork.Domain.Entities.TodoItems
{
    public class TodoItem : Entity, IHasDomainEvent, IAuditableEntity, IDeleteEntity
    {
        public int ListId { get; set; }

        public string Title { get; set; }

        public string Note { get; set; }

        public int? Priority { get; set; }

        public String PriorityLevelName => Priority.HasValue ? PriorityLevel.FromValue(Priority.Value).Name : "";

        public DateTime? Reminder { get; set; }

        private bool _done;

        public bool Done
        {
            get => _done;
            set
            {
                if (value == true && _done == false)
                {
                    DomainEvents.Add(new TodoItemCompletedEvent(this));
                }

                _done = value;
            }
        }

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();

        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}