using Microsoft.EntityFrameworkCore;
using SimpleBankAPI.Contracts;
using SimpleBankAPI.Core.Models;
using System.Linq.Expressions;

namespace SimpleBankAPI.Repository
{
    public class UserRepository : BaseRepository<User>, IBaseRepository<User>, IUserRepository
    {
        public UserRepository(SimpleBankAPIContext context, ILogger<UserRepository> logger) : base(context, logger) { }

        public virtual User? Get(int id) =>
            _context.Set<User>().Find(id);
        public virtual async Task<User?> GetAsync(int id) =>
            await _context.Set<User>().FindAsync(id);

    }
}