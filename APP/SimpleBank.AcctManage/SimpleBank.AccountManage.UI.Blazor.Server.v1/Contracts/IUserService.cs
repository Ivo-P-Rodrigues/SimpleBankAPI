using SimpleBank.AccountManage.UI.Blazor.Server.v1.Data.Requests;
using SimpleBank.AccountManage.UI.Blazor.Server.v1.Data.Responses;

namespace SimpleBank.AccountManage.UI.Blazor.Server.v1.Contracts
{
    public interface IUserService
    {
        Task<CreateUserResponse?> Create(CreateUserRequest createUserRequest);

        Task<CreateUserResponse?> GetUser();

        Task<bool> CheckLocallyIfUserIsLoggedAsync();
    }
}