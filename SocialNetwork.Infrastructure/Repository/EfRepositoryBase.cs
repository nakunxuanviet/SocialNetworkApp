using Microsoft.EntityFrameworkCore;
using NaKun.Arc.Domain.BaseEntity;
using NaKun.Arc.Domain.BaseRepository;
using NaKun.Arc.Domain.SeedWork;
using SocialNetwork.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SocialNetwork.Infrastructure.Repository
{
    public class EfRepositoryBase<T, TId> : IEfRepositoryBase<T, TId> where T : class, IEntityBase<TId>
    {
        private readonly DbFactory _dbFactory;
        private DbSet<T> _dbSet;

        protected DbSet<T> DbSet
        {
            //get => _dbSet ?? (_dbSet = _dbFactory.DbContext.Set<T>());
            get => _dbSet ??= _dbFactory.DbContext.Set<T>();
        }

        public EfRepositoryBase(DbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ? DbSet.AsNoTracking() : DbSet;

        public IQueryable<T> FindBy(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ? DbSet.Where(expression).AsNoTracking() : DbSet.Where(expression);

        public void Insert(T entity) => DbSet.Add(entity);

        public void Update(T entity) => DbSet.Update(entity);

        public void Delete(T entity)
        {
            if (typeof(IDeleteEntity).IsAssignableFrom(typeof(T)))
            {
                ((IDeleteEntity)entity).IsDeleted = true;
                DbSet.Update(entity);
            }
            else
                DbSet.Remove(entity);
        }

        public void AddRange(IEnumerable<T> entities) => DbSet.AddRange(entities);

        public void RemoveRange(IEnumerable<T> entities) => DbSet.RemoveRange(entities);
    }
}