using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SimpleBankAPI.Core.Models;
using System.Linq.Expressions;

namespace SimpleBankAPI.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : Entity
    {

        Task<TEntity?> DirectAddAsync(TEntity entity);
        Task<TEntity?> DirectUpdateAsync(TEntity entity);
        Task<TEntity?> DirectRemoveAsync(TEntity entity);
        EntityEntry<TEntity> Add(TEntity user);
        Task<EntityEntry<TEntity>> AddAsync(TEntity user);
        void AddRange(IEnumerable<TEntity> entities);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        void Dispose();
        bool Exists(Expression<Func<TEntity, bool>> predicate);
        TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();
        IEnumerable<TEntity> GetAllWhere(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAllWhereAsQueryable(Expression<Func<TEntity, bool>> predicate);
        TEntity? GetWhere(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> GetWhereAsync(Expression<Func<TEntity, bool>> predicate);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        EntityEntry<TEntity> Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
    }
}