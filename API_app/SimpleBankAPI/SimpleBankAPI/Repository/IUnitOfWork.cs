using SimpleBankAPI.Services;

namespace SimpleBankAPI.Repository
{
    public interface IUnitOfWork
    {
        IAccountRepository Accounts { get; }
        IMovementRepository Movements { get; }
        ITransferRepository Transfers { get; }
        IUserRepository Users { get; }
        IHardEntityMapper HardEntityMapper { get; }

        void ChangeEntityStateToModified<TEntity>(TEntity entity) where TEntity : class;
        void Dispose();
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}