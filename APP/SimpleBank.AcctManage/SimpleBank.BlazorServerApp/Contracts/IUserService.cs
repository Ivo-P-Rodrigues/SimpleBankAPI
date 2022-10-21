using SimpleBank.BlazorServerApp.Data.Requests;
using SimpleBank.BlazorServerApp.Data.Responses;

namespace SimpleBank.BlazorServerApp.Contracts
{
    public interface IUserService
    {
        Task<CreateUserResponse?> Create(CreateUserRequest createUserRequest);
        Task<bool> Login(LoginUserRequest loginUserRequest);
        Task<bool> Logout();
    }
}