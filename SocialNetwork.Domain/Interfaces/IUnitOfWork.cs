using SocialNetwork.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.SeedWork
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns></returns>    
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        IRepositoryBase<T> Repository<T>() where T : EntityBase;
    }
}