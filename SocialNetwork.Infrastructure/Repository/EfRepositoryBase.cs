using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Interfaces;
using SocialNetwork.Domain.SeedWork;
using SocialNetwork.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.Repository
{
    public class EfRepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
    {
        private readonly DbSet<T> _dbSet;

        public EfRepositoryBase(ApplicationDbContext dbContext)
        {
            _dbSet = dbContext.Set<T>();
        }

        public IQueryable<T> FindAll(bool trackChanges) => !trackChanges ? _dbSet.AsNoTracking() : _dbSet;

        public IQueryable<T> FindBy(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ? _dbSet.Where(expression).AsNoTracking() : _dbSet.Where(expression);

        public async Task<T> InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.FromResult(entity);
        }

        public Task<bool> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.FromResult(true);
        }

        public void AddRange(IEnumerable<T> entities) => _dbSet.AddRange(entities);

        public void DeleteRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);
    }
}