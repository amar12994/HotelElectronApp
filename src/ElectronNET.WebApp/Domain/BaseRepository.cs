using ElectronNET.WebApp.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ElectronNET.WebApp.Domain
{
    /// <summary>
    /// The class represent the generic repository. 
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>   
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly HotelContext _hotelContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <exception cref="ArgumentNullException">dbContext</exception>
        public BaseRepository(HotelContext hotelContext)
        {
            if (hotelContext == null)
            {
                throw new ArgumentNullException(nameof(hotelContext));
            }
            this._dbSet = hotelContext.Set<TEntity>();
            this._hotelContext = hotelContext;
        }

        /// <summary>
        /// Get all the records for the entity represented by TEntity.
        /// </summary>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>
        /// Return all the records for the TEntity.
        /// </returns>
        public async Task<ICollection<TEntity>> GetAll(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await this.GetQueryable(null, includeProperties).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get all the records for the entity as Queryable represented by TEntity.
        /// This method can be used to join and fire condtions on multiple tables.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>Return all the records for the TEntity.</returns>
        public IQueryable<TEntity> GetAllQueryable(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return this.GetQueryable(filter, includeProperties).AsNoTracking();
        }

        /// <summary>
        /// Get all the records with Entity Tracking. Use this method in case Tracking is most required, Otherwise use GetAll method. 
        /// </summary>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>Return all the records for the TEntity.</returns>
        public async Task<ICollection<TEntity>> GetAllWithTracking(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await this.GetQueryable(null, includeProperties).ToListAsync();
        }

        /// <summary>
        /// Get the record for id.
        /// </summary>
        /// <param name="id">The id of TEntity.</param>
        /// <returns>
        /// Return TEntity.
        /// </returns>
        public async Task<TEntity> GetById(int id)
        {
            return await _dbSet.SingleOrDefaultAsync(s => s.Id == id);
        }

        /// <summary>
        /// Add the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="ArgumentNullException">entity</exception>
        public async Task AddAsync(TEntity entity)
        {
            ValidateEntity(entity);
            await _dbSet.AddAsync(entity);
        }

        /// <summary>
        /// Add the list of TEntity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void AddRange(IEnumerable<TEntity> entity)
        {
            ValidateEntityList(entity);
            _dbSet.AddRange(entity);
        }

        /// <summary>
        /// Remove the list of TEntity.
        /// </summary>
        /// <param name="entitiesToRemove">The collection of entity.</param>
        public void RemoveRange(IEnumerable<TEntity> entitiesToRemove)
        {
            ValidateEntityList(entitiesToRemove);
            _dbSet.RemoveRange(entitiesToRemove);
        }

        /// <summary>
        /// Update the list of TEntity.
        /// </summary>
        /// <param name="entitiesToUpdate"></param>
        public void UpdateRange(IEnumerable<TEntity> entitiesToUpdate)
        {
            ValidateEntityList(entitiesToUpdate);
            _dbSet.UpdateRange(entitiesToUpdate);
        }

        /// <summary>
        /// Update the TEntity.
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TEntity entity)
        {
            ValidateEntity(entity);
            _dbSet.Update(entity);
        }

        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <exception cref="ArgumentNullException">entity</exception>
        public async Task Delete(int id)
        {
            TEntity entity = await _dbSet.SingleOrDefaultAsync(s => s.Id == id);
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Counts the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>Number of entities</returns>
        public async Task<int> Count(Expression<Func<TEntity, bool>> filter = null)
        {

            return await this.GetQueryable(filter).CountAsync();
        }

        /// <summary>
        /// Get any entities.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>Return any entities based on filter or without filter.</returns>
        public async Task<bool> Any(Expression<Func<TEntity, bool>> filter = null)
        {
            return await this.GetQueryable(filter).AnyAsync();
        }

        /// <summary>
        /// Finds the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>Find the entities based on filter or without filter.</returns>
        public async Task<ICollection<TEntity>> Find(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await this.GetQueryable(filter, includeProperties).ToListAsync();
        }

        /// <summary>
        /// Get distincts value for the entities.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The selector.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>Return distinct values</returns>
        public async Task<ICollection<TResult>> DistinctProperty<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> filter = null)
        {
            return await this.GetQueryable(filter).Select(selector).Distinct().ToListAsync();
        }

        #region private methods
        private IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> entities = this._dbSet;
            if (includeProperties is null)
            {
                throw new ArgumentNullException(nameof(includeProperties));
            }
            if (filter != null)
            {
                entities = entities.Where(filter);
            }

            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
            {
                entities = entities.Include(includeProperty);
            }

            return entities;
        }

        private static void ValidateEntity(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(typeof(TEntity).Name);
            }
        }

        private static void ValidateEntityList(IEnumerable<TEntity> entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(typeof(TEntity).Name);
            }
        }

        #endregion
    }
}
