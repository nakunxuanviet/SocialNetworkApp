using SocialNetwork.Domain.Entities.Activities;
using SocialNetwork.Infrastructure.Persistence;

namespace SocialNetwork.Infrastructure.Repository.ActivityRepository
{
    public class ActivityRepository : RepositoryBase<Activity>, IActivityRepository
    {
        public ActivityRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}