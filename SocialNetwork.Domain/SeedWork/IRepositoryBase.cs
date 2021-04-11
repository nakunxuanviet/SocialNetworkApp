using NaKun.Arc.Domain.BaseEntity;
using NaKun.Arc.Domain.BaseRepository;

namespace SocialNetwork.Domain.SeedWork
{
    public interface IRepositoryBase<T> : IEfRepositoryBase<T, int> where T : IEntityBase<int>
    {
    }
}
