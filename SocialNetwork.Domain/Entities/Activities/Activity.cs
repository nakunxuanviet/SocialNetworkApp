using SocialNetwork.Domain.SeedWork;
using System;
using System.Runtime.Serialization;

namespace SocialNetwork.Domain.Entities.Activities
{
    public class Activity : Entity, IAuditableEntity, IDeleteEntity
    {
        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// CategoryId
        /// </summary>
        public String Category { get; set; }

        /// <summary>
        /// CityId
        /// </summary>
        public String City { get; set; }

        /// <summary>
        /// Venue
        /// </summary>
        public string Venue { get; set; }

        /// <summary>
        /// Is Cancelled
        /// </summary>
        public bool IsCancelled { get; set; }

        [IgnoreDataMember]
        public bool IsDeleted { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}