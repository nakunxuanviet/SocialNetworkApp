using NaKun.Arc.Domain.BaseEntity;
using SocialNetwork.Domain.SeedWork;
using SocialNetwork.Infrastructure.Persistence;

namespace SocialNetwork.Infrastructure.Repository
{
    public class RepositoryBase<T> : EfRepositoryBase<T, int>, IRepositoryBase<T>
        where T : class, IEntityBase<int>
    {
        public RepositoryBase(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}