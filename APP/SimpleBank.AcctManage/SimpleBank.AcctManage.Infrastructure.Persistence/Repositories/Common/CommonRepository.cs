using Microsoft.EntityFrameworkCore;
using Serilog;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Core.Domain.Common;
using System;
using System.Linq.Expressions;

namespace SimpleBank.AcctManage.Infrastructure.Persistence.Repositories.Common
{
    public class CommonRepository<TEntity> : IDisposable, ICommonRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly SimpleBankDbContext _context;
        protected readonly ILogger _logger;

        public CommonRepository(
            SimpleBankDbContext context,
            ILogger logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
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
                _logger.Error("Error on creating new entity.", ex);
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
                _logger.Error("Error on updating entity.", ex);
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
                _logger.Error("Error on deleting entity.", ex);
                return null;
            }
            return entity;
        }



        //GET
        public virtual TEntity? Get(Guid id) =>
            _context.Set<TEntity>().Find(id);
        public virtual async Task<TEntity?> GetAsync(Guid id) =>
            await _context.Set<TEntity>().FindAsync(id);
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
        public virtual void Add(TEntity user) =>
            _context.Set<TEntity>().Add(user);
        public virtual async Task AddAsync(TEntity user) =>
            await _context.Set<TEntity>().AddAsync(user);
        public virtual void AddRange(IEnumerable<TEntity> entities) =>
            _context.Set<TEntity>().AddRange(entities);
        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities) =>
            await _context.Set<TEntity>().AddRangeAsync(entities);


        //UPDATE - tracking
        public virtual void Update(TEntity entity) =>
            _context.Set<TEntity>().Update(entity);
        public virtual void UpdateRange(IEnumerable<TEntity> entities) =>
            _context.Set<TEntity>().UpdateRange(entities);


        //DELETE - tracking
        public virtual void Remove(TEntity entity) =>
            _context.Set<TEntity>().Remove(entity);
        public virtual void RemoveRange(IEnumerable<TEntity> entities) =>
            _context.Set<TEntity>().RemoveRange(entities);


        //misc
        public virtual void Dispose() =>
            _context.Dispose();
        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate) =>
            _context.Set<TEntity>().Any(predicate);

        public virtual void UntrackEntity(TEntity entity) =>
            _context.Entry(entity).State = EntityState.Detached;

    }
}
