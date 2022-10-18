using SimpleBank.BlazorServerApp.Data.Requests;
using SimpleBank.BlazorServerApp.Data.Responses;

namespace SimpleBank.BlazorServerApp.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse?> Create(CreateUserRequest createUserRequest);
        Task<LoginUserResponse?> Login(LoginUserRequest loginUserRequest);
        Task<string?> Logout(LogoutUserRequest logoutUserRequest, string accessToken);
        Task<LoginUserResponse?> RenewToken(RenewRequest renewRequest);
    }
}