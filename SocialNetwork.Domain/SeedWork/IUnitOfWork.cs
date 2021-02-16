using System;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.SeedWork
{
    public interface IUnitOfWork : IDisposable
    {
        //Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        //Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default);
        Task<int> CommitAsync();
    }
}