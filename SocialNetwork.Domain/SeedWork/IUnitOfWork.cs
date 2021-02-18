using System;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.SeedWork
{
    public interface IUnitOfWork
    {
        Task<int> CommitChangeAsync();
    }
}