using SimpleBank.BlazorServerApp.Data.Requests;
using SimpleBank.BlazorServerApp.Data.Responses;

namespace SimpleBank.BlazorServerApp.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse?> NewUser(CreateUserRequest createUserRequest);
    }
}