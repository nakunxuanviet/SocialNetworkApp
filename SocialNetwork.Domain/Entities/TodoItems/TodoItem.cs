using SocialNetwork.Domain.SeedWork;
using System;

namespace SocialNetwork.Domain.Entities.TodoItems
{
    public class TodoItem : EntityBase<int>, IAuditableEntity, IDeleteEntity
    {
        public int ListId { get; set; }

        public string Title { get; set; }

        public string Note { get; set; }

        public int? Priority { get; set; }

        public String PriorityLevelName => Priority.HasValue ? PriorityLevel.FromValue(Priority.Value).Name : "";

        public DateTime? Reminder { get; set; }

        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}