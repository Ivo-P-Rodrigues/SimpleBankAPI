using SimpleBank.BlazorServerApp.Data.Requests;
using SimpleBank.BlazorServerApp.Data.Responses;

namespace SimpleBank.BlazorServerApp.Contracts
{
    public interface IUserService
    {
        Task<CreateUserResponse?> Create(CreateUserRequest createUserRequest);
        Task<LoginUserResponse?> Login(LoginUserRequest loginUserRequest);
        Task<string?> Logout();
        Task<LoginUserResponse?> RenewToken(RenewRequest renewRequest);
    }
}