using Microsoft.EntityFrameworkCore;
using SimpleBank.AcctManage.Core.Domain;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Infrastructure.Persistence.Repositories.Common;
using Serilog;

namespace SimpleBank.AcctManage.Infrastructure.Persistence.Repositories
{
    public class SessionRepository : CommonRepository<Session>, ISessionRepository
    {
        public SessionRepository(SimpleBankDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<Session?> GetLastSessionOfUserAsync(Guid userId) =>
            await GetAllWhereAsQueryable(s => s.UserId == userId).OrderByDescending(s => s.CreatedAt).FirstOrDefaultAsync();
        public Session? GetLastSessionOfUser(Guid userId) =>
            GetAllWhereAsQueryable(s => s.UserId == userId).OrderByDescending(s => s.CreatedAt).FirstOrDefault();




    }
}
