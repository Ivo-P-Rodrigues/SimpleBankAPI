using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Business
{
    public interface ISessionBusiness
    {
        Task<Session?> ProcessLoginAsync(Guid userId);
        Task<bool> ProcessLogoutAsync(Session session);
        Task<Session?> RefreshSessionAsync(Guid userId, string refreshToken);
        bool VerifyLogoutRequestObj(Guid userId, Guid sessionId, out Session session);
    }
}