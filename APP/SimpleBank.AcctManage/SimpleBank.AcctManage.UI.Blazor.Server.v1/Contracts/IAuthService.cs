using SimpleBank.AcctManage.UI.Blazor.Server.v1.Data.Requests;

namespace SimpleBank.AcctManage.UI.Blazor.Server.v1.Contracts
{
    public interface IAuthService
    {
        Task<bool> Login(LoginUserRequest loginUserRequest);
        Task<bool> Logout();
    }
}