using Microsoft.EntityFrameworkCore;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Domain;
using SimpleBank.AcctManage.Persistence.Repositories.Common;

namespace SimpleBank.AcctManage.Persistence.Repositories
{
    public class SessionRepository : CommonRepository<Session>, ISessionRepository
    {
        public SessionRepository(SimpleBankDbContext context) : base(context)
        {
        }

        public async Task<Session?> GetLastSessionOfUserAsync(Guid userId) =>
            await GetAllWhereAsQueryable(s => s.UserId == userId).OrderByDescending(s => s.CreatedAt).FirstOrDefaultAsync();
        public Session? GetLastSessionOfUser(Guid userId) =>
            GetAllWhereAsQueryable(s => s.UserId == userId).OrderByDescending(s => s.CreatedAt).FirstOrDefault();




    }
}
