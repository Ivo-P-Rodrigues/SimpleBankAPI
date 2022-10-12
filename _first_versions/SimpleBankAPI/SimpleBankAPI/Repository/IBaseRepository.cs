using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SimpleBankAPI.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Changes the state of the entity that is being tracked by a context.<para />
        /// 'state': Detached = 0 | Unchanged = 1 | Deleted = 2 | Modified = 3 | Added = 4 .<para />
        /// Set to default 3 : Modified.
        /// </summary>
        /// <param name="entity"><see cref="TEntity"/> object to change state.</param>
        /// <param name="state">The state to change to. </param>
        void ChangeEntityState(TEntity entity, int state = 3);
        /// <summary>
        /// Revert entities to Unchanged. Works with ALL types of model entities.
        /// </summary>
        /// <param name="entities">Entities to revert to Unchanged.</param>
        Task RevertAllEntitiesState(IQueryable<TEntity> entities);
        /// <summary>
        /// Revert entities to Unchanged. Works with ALL types of model entities.
        /// </summary>
        /// <param name="entities">Entities to revert to Unchanged.</param>
        void RevertAllEntitiesState(List<TEntity> entities);

        bool Exists(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity user);
        Task AddAsync(TEntity user);
        void AddRange(IEnumerable<TEntity> users);
        Task AddRangeAsync(IEnumerable<TEntity> users);
        TEntity? Get(Expression<Func<TEntity, bool>> predicate);
        TEntity? Get(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> GetAsync(int id);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}