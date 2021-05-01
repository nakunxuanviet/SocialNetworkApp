using System;
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
        Task<int> CommitChangeAsync(CancellationToken cancellationToken);
    }
}