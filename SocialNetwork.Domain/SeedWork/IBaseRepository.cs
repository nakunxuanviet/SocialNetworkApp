using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.SeedWork
{
    public interface IBaseRepository<T> where T : class
    {
        IQueryable<T> FindAll(bool trackChanges);

        IQueryable<T> FindBy(Expression<Func<T, bool>> expression, bool trackChanges);

        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        void AddRange(IEnumerable<T> entities);

        void RemoveRange(IEnumerable<T> entities);
    }
}