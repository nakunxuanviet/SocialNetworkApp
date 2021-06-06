using SocialNetwork.Domain.SeedWork;
using System;
using System.Runtime.Serialization;

namespace SocialNetwork.Domain.Entities.Activities
{
    public class City : EntityBase, IAuditableEntity, IDeleteEntity
    {
        public string Name { get; set; }
        public Activity Activity { get; set; }

        [IgnoreDataMember]
        public bool IsDeleted { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}