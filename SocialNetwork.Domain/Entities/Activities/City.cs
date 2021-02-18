using SocialNetwork.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.Entities.Activities
{
    public class City : Entity, IAuditableEntity, IDeleteEntity
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