using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;

namespace SimpleBank.AcctManage.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SimpleBankDbContext _context;

        public UnitOfWork(
            SimpleBankDbContext context,
            IAccountRepository accounts,
            IMovementRepository movements,
            ITransferRepository transfers,
            IUserRepository users,
            ISessionRepository sessions)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Accounts = accounts ?? throw new ArgumentNullException(nameof(accounts));
            Movements = movements ?? throw new ArgumentNullException(nameof(movements));
            Transfers = transfers ?? throw new ArgumentNullException(nameof(transfers));
            Users = users ?? throw new ArgumentNullException(nameof(users));
            Sessions = sessions ?? throw new ArgumentNullException(nameof(sessions));
        }


        public IAccountRepository Accounts { get; private set; }
        public IMovementRepository Movements { get; private set; }
        public ITransferRepository Transfers { get; private set; }
        public IUserRepository Users { get; private set; }
        public ISessionRepository Sessions { get; private set; }


        public int SaveChanges() =>
            _context.SaveChanges();
        public async Task<int> SaveChangesAsync() =>
            await _context.SaveChangesAsync();
        public void Dispose() =>
            _context.Dispose();


    }
}
