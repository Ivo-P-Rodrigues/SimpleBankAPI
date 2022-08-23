using Microsoft.EntityFrameworkCore;
using SimpleBankAPI.Models;
using SimpleBankAPI.Services;
using System.Linq.Expressions;

namespace SimpleBankAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly SimpleBankAPIContext _context;

        public UnitOfWork(
            SimpleBankAPIContext context,
            IAccountRepository accounts,
            IMovementRepository movements,
            ITransferRepository transfers,
            IUserRepository users,
            IHardEntityMapper hardEntityMapper)
        {
            _context = context;
            Accounts = accounts;
            Movements = movements;
            Transfers = transfers;
            Users = users;
            HardEntityMapper = hardEntityMapper;
        }

        public IAccountRepository Accounts { get; private set; }
        public IMovementRepository Movements { get; private set; }
        public ITransferRepository Transfers { get; private set; }
        public IUserRepository Users { get; private set; }
        public IHardEntityMapper HardEntityMapper { get; private set; }



        public void ChangeEntityStateToModified<TEntity>(TEntity entity) where TEntity : class =>
            _context.Entry(entity).State = EntityState.Modified;



        //general 
        public int SaveChanges() =>
            _context.SaveChanges();
        public async Task<int> SaveChangesAsync() =>
            await _context.SaveChangesAsync();
        public void Dispose() =>
            _context.Dispose();





    }
}



