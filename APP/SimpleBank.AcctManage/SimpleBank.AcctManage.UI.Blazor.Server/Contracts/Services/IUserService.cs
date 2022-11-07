using SimpleBank.AcctManage.UI.Blazor.Server.Data;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Contracts.Services
{
    public interface IUserService
    {
        Task<bool> Create(UserCreate createUserRequest);
    }
}