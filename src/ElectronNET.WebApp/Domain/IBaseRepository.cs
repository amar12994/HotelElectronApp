using ElectronNET.WebApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ElectronNET.WebApp.Domain
{
    /// <summary>
    /// This interface contains only methods which are specific to BaseRepository.
    /// </summary>   
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// Get all the records for the entity represented by TEntity.
        /// </summary>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>Return all the records for the TEntity.</returns>
        Task<ICollection<TEntity>> GetAll(params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Get all the records for the entity as Queryable represented by TEntity.
        /// This method can be used to join and fire condtions on multiple tables.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>Return all the records for the TEntity.</returns>
        IQueryable<TEntity> GetAllQueryable(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Get all the records with Entity Tracking. Use this method in case Tracking is most required, Otherwise use GetAll method. 
        /// </summary>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>Return all the records for the TEntity.</returns>
        Task<ICollection<TEntity>> GetAllWithTracking(params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Get the record for id.
        /// </summary>
        /// <param name="id">The id of TEntity.</param>
        /// <returns>Return TEntity.</returns>
        Task<TEntity> GetById(int id);

        /// <summary>
        /// Add the list of TEntity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void AddRange(IEnumerable<TEntity> entity);

        /// <summary>
        /// Remove the list of TEntity.
        /// </summary>
        /// <param name="entitiesToRemove">The collection of entity.</param>
        void RemoveRange(IEnumerable<TEntity> entitiesToRemove);

        /// <summary>
        /// Update the list of TEntity.
        /// </summary>
        /// <param name="entitiesToRemove">The collection of entity.</param>
        void UpdateRange(IEnumerable<TEntity> entitiesToUpdate);

        /// <summary>
        /// Update the TEntity.
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TEntity entity);

        /// <summary>
        /// Add the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        Task Delete(int id);

        /// <summary>
        /// Counts the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        Task<int> Count(Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// Anies the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        Task<bool> Any(Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// Distincts the property.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        Task<ICollection<TResult>> DistinctProperty<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// Finds the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns></returns>
        Task<ICollection<TEntity>> Find(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includeProperties);
    }
}