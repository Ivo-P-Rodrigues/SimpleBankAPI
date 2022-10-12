using Microsoft.EntityFrameworkCore;
using SimpleBankAPI.Core.Models;
using System.Linq.Expressions;

namespace SimpleBankAPI.Repository
{
    public class MovementRepository : BaseRepository<Movement>, IBaseRepository<Movement>, IMovementRepository
    {
        public MovementRepository(SimpleBankAPIContext context, ILogger<MovementRepository> logger) : base(context, logger) { }

        public virtual Movement? Get(int id) =>
            _context.Set<Movement>().Find(id);
        public virtual async Task<Movement?> GetAsync(int id) =>
            await _context.Set<Movement>().FindAsync(id);

    }
}