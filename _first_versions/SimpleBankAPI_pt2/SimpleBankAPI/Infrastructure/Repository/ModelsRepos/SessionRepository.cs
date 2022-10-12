using Microsoft.EntityFrameworkCore;
using SimpleBankAPI.Core.Models;

namespace SimpleBankAPI.Repository
{
    public class SessionRepository : BaseRepository<Session>, IBaseRepository<Session>, ISessionRepository
    {
        public SessionRepository(SimpleBankAPIContext context, ILogger<SessionRepository> logger) : base(context, logger) { }

        public virtual Session? Get(string id) =>
            _context.Set<Session>().Find(id);
        public virtual async Task<Session?> GetAsync(string id) =>
            await _context.Set<Session>().FindAsync(id);

        public async Task<Session?> GetLastSessionOfUserAsync(int userId) =>
            await GetAllWhereAsQueryable(s => s.UserId == userId).OrderByDescending(s => s.CreatedAt).FirstOrDefaultAsync();
        public Session? GetLastSessionOfUser(int userId) =>
            GetAllWhereAsQueryable(s => s.UserId == userId).OrderByDescending(s => s.CreatedAt).FirstOrDefault();

    }
}