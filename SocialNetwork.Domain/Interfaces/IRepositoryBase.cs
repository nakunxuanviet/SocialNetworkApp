using SocialNetwork.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.Interfaces
{
    public interface IRepositoryBase<T> where T : EntityBase
    {
        /// <summary>
        /// Find all entities in a table.
        /// The returned data type is queryable.
        /// If trackChanges = true then add asNoTracking() for optimize performance.
        /// </summary>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        IQueryable<T> FindAll(bool trackChanges);

        /// <summary>
        /// Find entities with conditional expression.
        /// The returned data type is queryable.
        /// If trackChanges = true then add asNoTracking() for optimize performance.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        IQueryable<T> FindBy(Expression<Func<T, bool>> expression, bool trackChanges);

        /// <summary>
        /// Insert a entity
        /// </summary>
        /// <param name="entity"></param>
        Task<T> InsertAsync(T entity);

        /// <summary>
        /// Update a entity
        /// </summary>
        /// <param name="entity"></param>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Delete a entity
        /// </summary>
        /// <param name="entity"></param>
        Task<bool> DeleteAsync(T entity);

        /// <summary>
        /// Add a list of entities
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(IEnumerable<T> entities);

        /// <summary>
        /// Delete a list of entities
        /// </summary>
        /// <param name="entities"></param>
        void DeleteRange(IEnumerable<T> entities);

        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);

        Task<bool> Exists(Expression<Func<T, bool>> expression);
    }
}
