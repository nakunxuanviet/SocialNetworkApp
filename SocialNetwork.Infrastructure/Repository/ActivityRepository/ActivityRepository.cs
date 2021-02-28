using SocialNetwork.Domain.Entities.Activities;
using SocialNetwork.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.Repository.ActivityRepository
{
    public class ActivityRepository : BaseRepository<Activity>, IActivityRepository
    {
        public ActivityRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}