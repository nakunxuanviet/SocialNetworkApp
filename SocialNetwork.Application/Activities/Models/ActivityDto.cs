﻿using SocialNetwork.Application.Common.Mappings;
using SocialNetwork.Domain.Entities.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Activities.Models
{
    public class ActivityDto : IMapFrom<Activity>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string City { get; set; }

        public string Venue { get; set; }

        public bool IsCancelled { get; set; }
    }
}