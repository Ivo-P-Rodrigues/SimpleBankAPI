using SimpleBankAPI.WebAPI;

namespace SimpleBankAPI.Repository
{
    public interface IUnitOfWork
    {
        IAccountRepository Accounts { get; }
        IMovementRepository Movements { get; }
        ITransferRepository Transfers { get; }
        ISessionRepository Sessions { get; }
        IUserRepository Users { get; }

        void Dispose();
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}