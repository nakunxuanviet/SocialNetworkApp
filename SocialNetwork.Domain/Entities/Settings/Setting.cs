using SocialNetwork.Domain.SeedWork;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SocialNetwork.Domain.Entities.Settings
{
    public class Setting : BaseEntity, IAuditableEntity, IDeleteEntity, IAggregateRoot
    {
        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }

        [IgnoreDataMember]
        [DefaultValue(0)]
        public bool IsDeleted { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}