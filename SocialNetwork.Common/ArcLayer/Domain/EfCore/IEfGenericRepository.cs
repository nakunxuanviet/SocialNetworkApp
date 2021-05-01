using SocialNetwork.Common.ArcLayer.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SocialNetwork.Common.ArcLayer.Domain.EfCore
{
    /// <summary>
    /// Genenic repository for Entity Framework
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEfGenericRepository<TEntity, TId> where TEntity : IEntityBase<TId>
    {
        /// <summary>
        /// Find all entities in a table.
        /// The returned data type is queryable.
        /// If trackChanges = true then add asNoTracking() for optimize performance.
        /// </summary>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        IQueryable<TEntity> FindAll(bool trackChanges);

        /// <summary>
        /// Find entities with conditional expression.
        /// The returned data type is queryable.
        /// If trackChanges = true then add asNoTracking() for optimize performance.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> expression, bool trackChanges);

        /// <summary>
        /// Insert a entity
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity);

        /// <summary>
        /// Update a entity
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// Delete a entity
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);

        /// <summary>
        /// Add a list of entities
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Delete a list of entities
        /// </summary>
        /// <param name="entities"></param>
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}