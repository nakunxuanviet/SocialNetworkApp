using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.SeedWork
{
    /// <summary>
    /// Genenic repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> where T : class
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
        void Create(T entity);

        /// <summary>
        /// Update a entity
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        /// Delete a entity
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);

        /// <summary>
        /// Add a list of entities
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(IEnumerable<T> entities);

        /// <summary>
        /// Delete a list of entities
        /// </summary>
        /// <param name="entities"></param>
        void RemoveRange(IEnumerable<T> entities);
    }
}