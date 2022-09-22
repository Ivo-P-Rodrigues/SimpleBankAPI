using SimpleBankAPI.Contracts;
using SimpleBankAPI.Core.Models;

namespace SimpleBankAPI.Business
{
    public interface ISessionBusiness
    {
        Task<Session?> ProcessLoginAsync(int userId);
        Task<bool> ProcessLogoutAsync(Session session);
        Task<Session?> RefreshSessionAsync(int userId, string refreshToken);
        bool VerifyLogoutRequestObj(int userId, LogoutUserRequest logoutUserRequest, out Session session);

    }
}