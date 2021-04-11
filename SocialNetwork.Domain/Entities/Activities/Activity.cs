using NaKun.Arc.Domain.Exceptions;
using NaKun.Arc.Domain.SeedWork;
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
        private DateTime _date;

        public DateTime Date
        {
            get => _date;
            set
            {
                if (value == DateTime.MinValue)
                    throw new DomainException("Date is required ot");
                _date = value;
            }
        }

        /// <summary>
        /// Description
        /// </summary>
        private string _description;

        public string Description
        {
            get => _description;
            set => _description = string.IsNullOrEmpty(value) ? null : value;
        }

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
        private string _venue;

        public string Venue
        {
            get => _venue;
            set => _venue = string.IsNullOrEmpty(value) ? null : value;
        }

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