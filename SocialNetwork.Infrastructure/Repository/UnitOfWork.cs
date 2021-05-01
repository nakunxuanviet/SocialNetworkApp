using SocialNetwork.Domain.SeedWork;
using SocialNetwork.Infrastructure.Persistence;
using System;
using System.Collections;
using System.Threading;
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

        public Task<int> CommitChangeAsync(CancellationToken cancellationToken)
        {
            return _dbFactory.DbContext.SaveChangesAsync();
        }
    }

    //private readonly ApplicationDbContext _context;
    //private Hashtable _repositories;

    //public UnitOfWork(ApplicationDbContext context)
    //{
    //    _context = context;
    //}

    //public async Task<int> CompleteAsync()
    //{
    //    return await _context.SaveChangesAsync();
    //}

    //public void Dispose()
    //{
    //    _context.Dispose();
    //}

    ///// Using: await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
    //public IRepositoryBase<TEntity> Repository<TEntity>() where TEntity : Entity
    //{
    //    if (_repositories == null) _repositories = new Hashtable();

    //    var type = typeof(TEntity).Name;

    //    if (!_repositories.ContainsKey(type))
    //    {
    //        var repositoryType = typeof(IRepositoryBase<>);
    //        var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

    //        _repositories.Add(type, repositoryInstance);
    //    }

    //    return (IRepositoryBase<TEntity>)_repositories[type];
    //}
}