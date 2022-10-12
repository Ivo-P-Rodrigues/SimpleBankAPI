using Microsoft.EntityFrameworkCore;
using SimpleBankAPI.Core.Models;
using System.Linq.Expressions;

namespace SimpleBankAPI.Repository
{
    public class AccountRepository : BaseRepository<Account>, IBaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(SimpleBankAPIContext context, ILogger<AccountRepository> logger) : base(context, logger) { }

        public virtual Account? Get(int id) =>
            _context.Set<Account>().Find(id);
        public virtual async Task<Account?> GetAsync(int id) =>
            await _context.Set<Account>().FindAsync(id);
    }
}