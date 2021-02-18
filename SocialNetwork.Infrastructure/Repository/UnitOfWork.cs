using SocialNetwork.Domain.SeedWork;
using SocialNetwork.Infrastructure.Persistence;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbFactory _dbFactory;

        public UnitOfWork(DbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public Task<int> CommitChangeAsync()
        {
            return _dbFactory.DbContext.SaveChangesAsync();
        }
    }
}