using SocialNetwork.Domain.Entities.Activities;
using SocialNetwork.Infrastructure.Persistence;

namespace SocialNetwork.Infrastructure.Repository.ActivityRepository
{
    public class ActivityRepository : EfRepositoryBase<Activity>, IActivityRepository
    {
        public ActivityRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}