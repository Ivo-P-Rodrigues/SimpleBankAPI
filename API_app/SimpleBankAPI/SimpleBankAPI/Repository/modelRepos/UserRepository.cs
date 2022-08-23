using Microsoft.EntityFrameworkCore;
using SimpleBankAPI.Models;
using System.Linq.Expressions;

namespace SimpleBankAPI.Repository
{
    public class UserRepository : BaseRepository<User>, IBaseRepository<User>, IUserRepository
    {
        private readonly SimpleBankAPIContext _context;
        public UserRepository(SimpleBankAPIContext context) : base(context) =>
            _context = context ?? throw new ArgumentNullException(nameof(context));


        //user state
        public bool Exists(int id) =>
            _context.Set<User>().Any(e => e.UserId == id);







    }
}