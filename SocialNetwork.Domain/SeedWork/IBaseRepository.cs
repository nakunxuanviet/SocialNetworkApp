using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.SeedWork
{
    public interface IBaseRepository<T> where T : class
    {
        IQueryable<T> QueryAll();

        IQueryable<T> Query(Expression<Func<T, bool>> expression);

        Task<T> AddAsync(T entity);

        void AddRange(IEnumerable<T> entities);

        void Update(T entity);

        void Delete(T entity);

        void RemoveRange(IEnumerable<T> entities);
    }
}