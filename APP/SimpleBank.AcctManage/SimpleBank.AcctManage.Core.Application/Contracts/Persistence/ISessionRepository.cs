using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Persistence
{
    public interface ISessionRepository : ICommonRepository<Session>
    {
        Task<Session?> GetLastSessionOfUserAsync(Guid userId);
        Session? GetLastSessionOfUser(Guid userId);

    }
}
