namespace SimpleBank.AcctManage.Core.Application.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        IAccountRepository Accounts { get; }
        IMovementRepository Movements { get; }
        ITransferRepository Transfers { get; }
        IUserRepository Users { get; }
        IUserTokenRepository UserTokens { get; }


   
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void Dispose();




    }
}
