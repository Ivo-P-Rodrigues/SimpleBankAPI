using SimpleBankAPI.Contracts;
using SimpleBankAPI.Models;

namespace SimpleBankAPI.Business
{
    public interface IUserBusiness
    {
        Task<CreateUserResponse?> Create(CreateUserRequest createUserRequest);
        Task<string?> ProcessLogin(LoginUserRequest loginUserRequest);
        bool CheckUsernamePasswordCombo(LoginUserRequest loginUserRequest);
        User? GetUserByUsernamePasswordCombo(LoginUserRequest loginUserRequest);
        Task<User?> GetUserByUsernamePasswordComboAsync(LoginUserRequest loginUserRequest);

    }
}