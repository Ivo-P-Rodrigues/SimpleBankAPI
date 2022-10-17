using SimpleBank.AcctManage.Core.Domain.Common;
using System.Linq.Expressions;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Persistence
{
    public interface ICommonRepository<TEntity> where TEntity : class
    {
        TEntity? Get(Guid id);
        Task<TEntity?> GetAsync(Guid id);
        void Add(TEntity user);
        Task AddAsync(TEntity user);
        void AddRange(IEnumerable<TEntity> entities);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task<TEntity?> DirectAddAsync(TEntity entity);
        Task<TEntity?> DirectRemoveAsync(TEntity entity);
        Task<TEntity?> DirectUpdateAsync(TEntity entity);
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
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void UntrackEntity(TEntity entity);
    }
}