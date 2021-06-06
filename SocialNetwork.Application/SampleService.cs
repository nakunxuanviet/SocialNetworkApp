using SocialNetwork.Domain.Entities.Activities;
using SocialNetwork.Domain.SeedWork;
using System.Threading.Tasks;

namespace SocialNetwork.Application
{
    public class SampleService : BaseService
    {
        public SampleService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<Activity> AddNewAsync(Activity model)
        {
            var act = new Activity();

            var repository = UnitOfWork.Repository<Activity>();
            await repository.InsertAsync(act);
            await UnitOfWork.SaveChangesAsync();

            return act;
        }
    }
}
