using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SimpleBankAPI.Core.Models;
using System.Linq.Expressions;

namespace SimpleBankAPI.Repository
{
    public abstract class BaseRepository<TEntity> : DbContext, IDisposable, IBaseRepository<TEntity> where TEntity : Entity
    {

        protected readonly DbContext _context;
        protected readonly ILogger _logger;
        public BaseRepository(
            DbContext context,
            ILogger logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        //direct crud - immediately sets the crud action against the DB (not just the tracking) 
        public async Task<TEntity?> DirectAddAsync(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Error on creating new entity.", ex);
                return null;
            }
            return entity;
        }
        public async Task<TEntity?> DirectUpdateAsync(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Error on updating entity", ex);
                return null;
            }
            return entity;
        }
        public async Task<TEntity?> DirectRemoveAsync(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Error on removing entity.", ex);
                return null;
            }
            return entity;
        }


        //misc
        public virtual void Dispose() =>
            _context.Dispose();
        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate) =>
            _context.Set<TEntity>().Any(predicate);


        //GET
        public virtual IEnumerable<TEntity> GetAll() =>
            _context.Set<TEntity>().ToList();
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync() =>
            await _context.Set<TEntity>().ToListAsync();


        //GET Where
        public virtual TEntity? GetWhere(Expression<Func<TEntity, bool>> predicate) =>
            _context.Set<TEntity>().FirstOrDefault(predicate);
        public virtual async Task<TEntity?> GetWhereAsync(Expression<Func<TEntity, bool>> predicate) =>
            await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        public virtual IEnumerable<TEntity> GetAllWhere(Expression<Func<TEntity, bool>> predicate) =>
            _context.Set<TEntity>().Where(predicate);
        public virtual IQueryable<TEntity> GetAllWhereAsQueryable(Expression<Func<TEntity, bool>> predicate) =>
            _context.Set<TEntity>().Where(predicate);


        //GET extra
        public TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate) =>
            _context.Set<TEntity>().FirstOrDefault(predicate);


        //ADD - tracking
        public virtual EntityEntry<TEntity> Add(TEntity user) =>
            _context.Set<TEntity>().Add(user);
        public virtual async Task<EntityEntry<TEntity>> AddAsync(TEntity user) =>
            await _context.Set<TEntity>().AddAsync(user);
        public virtual void AddRange(IEnumerable<TEntity> entities) =>
            _context.Set<TEntity>().AddRange(entities);
        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities) =>
            await _context.Set<TEntity>().AddRangeAsync(entities);


        //UPDATE - tracking
        public virtual EntityEntry<TEntity> Update(TEntity entity) =>
            _context.Set<TEntity>().Update(entity);
        public virtual void UpdateRange(IEnumerable<TEntity> entities) =>
            _context.Set<TEntity>().UpdateRange(entities);


        //DELETE - tracking
        public virtual void Remove(TEntity entity) =>
            _context.Set<TEntity>().Remove(entity);
        public virtual void RemoveRange(IEnumerable<TEntity> entities) =>
            _context.Set<TEntity>().RemoveRange(entities);


    }
}
