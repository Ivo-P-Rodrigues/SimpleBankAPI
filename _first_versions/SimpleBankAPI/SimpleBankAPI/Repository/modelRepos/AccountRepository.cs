using Microsoft.EntityFrameworkCore;
using SimpleBankAPI.Models;
using System.Linq.Expressions;

namespace SimpleBankAPI.Repository
{
    public class AccountRepository : BaseRepository<Account>, IBaseRepository<Account>, IAccountRepository
    {
        private readonly SimpleBankAPIContext _context;
        public AccountRepository(SimpleBankAPIContext context) : base(context) =>
            _context = context ?? throw new ArgumentNullException(nameof(context));


        //user state
        public bool Exists(int id) =>
            _context.Set<Account>().Any(e => e.AccountId == id);




    }
}