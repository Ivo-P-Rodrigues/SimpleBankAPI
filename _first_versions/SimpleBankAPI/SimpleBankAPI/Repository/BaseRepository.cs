using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleBankAPI.Models;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SimpleBankAPI.Repository
{
    public abstract class BaseRepository<TEntity> : DbContext, IBaseRepository<TEntity> where TEntity : class
    {

        protected readonly DbContext _context;
        public BaseRepository(DbContext context) =>
            _context = context ?? throw new ArgumentNullException(nameof(context));


#pragma warning disable CS8603
        // Possible null reference return.


        //TEntity state
        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate) =>
            _context.Set<TEntity>().Any(predicate);

        /// <summary>
        /// Changes the state of the entity that is being tracked by a context.<para />
        /// 'state': Detached = 0 | Unchanged = 1 | Deleted = 2 | Modified = 3 | Added = 4 .<para />
        /// Set to default 3 : Modified.
        /// </summary>
        /// <param name="entity"><see cref="TEntity"/> object to change state.</param>
        /// <param name="state">The state to change to. </param>
        public virtual void ChangeEntityState(TEntity entity, int state = 3) =>
            _context.Entry(entity).State = (EntityState)state;
        /// <summary>
        /// Revert entities to Unchanged. Works with ALL types of model entities.
        /// </summary>
        /// <param name="entities">Entities to revert to Unchanged.</param>
        public virtual async Task RevertAllEntitiesState(IQueryable<TEntity> entities) =>
            await entities.ForEachAsync(entity => _context.Entry(entity).State = (EntityState)1);
        /// <summary>
        /// Revert entities to Unchanged. Works with ALL types of model entities.
        /// </summary>
        /// <param name="entities">Entities to revert to Unchanged.</param>
        public virtual void RevertAllEntitiesState(List<TEntity> entities) =>
            entities.ForEach(entity => _context.Entry(entity).State = (EntityState)1);


        //GET
        public virtual TEntity? Get(int id) =>
            _context.Set<TEntity>().Find(id);
        public virtual TEntity? Get(Expression<Func<TEntity, bool>> predicate) =>
            _context.Set<TEntity>().FirstOrDefault(predicate);
        public virtual async Task<TEntity?> GetAsync(int id) =>
            await _context.Set<TEntity>().FindAsync(id);
        public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate) =>
            await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);

        public virtual IEnumerable<TEntity> GetAll() =>
            _context.Set<TEntity>().ToList();
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync() =>
            await _context.Set<TEntity>().ToListAsync();
        public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate) =>
            _context.Set<TEntity>().Where(predicate);
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate) =>
            await _context.Set<TEntity>().Where(predicate).ToListAsync();


        //ADD
        public virtual void Add(TEntity user) =>
            _context.Set<TEntity>().Add(user);
        public virtual async Task AddAsync(TEntity user) =>
            await _context.Set<TEntity>().AddAsync(user);
        public virtual void AddRange(IEnumerable<TEntity> entities) =>
            _context.Set<TEntity>().AddRange(entities);
        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities) =>
            await _context.Set<TEntity>().AddRangeAsync(entities);


        //DELETE
        public virtual void Remove(TEntity entity) =>
            _context.Set<TEntity>().Remove(entity);
        public virtual void RemoveRange(IEnumerable<TEntity> entities) =>
            _context.Set<TEntity>().RemoveRange(entities);



#pragma warning restore CS8603
    }
}
