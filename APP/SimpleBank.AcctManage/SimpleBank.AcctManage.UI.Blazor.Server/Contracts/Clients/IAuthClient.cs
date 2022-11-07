using SimpleBank.AcctManage.UI.Blazor.Server.Data;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Contracts.Clients
{
    public interface IAuthClient
    {
        Task<bool> Login(UserLogin userLogin);
        Task<bool> Logout();
        Task<object?> RefreshAsync();
    }
}