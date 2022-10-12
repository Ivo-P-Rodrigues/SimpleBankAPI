using SimpleBankAPI.Contracts;
using SimpleBankAPI.Core.Models;

namespace SimpleBankAPI.Business
{
    public interface IUserBusiness
    {
        Task<User?> CreateUser(User user, string password);
        bool VerifyUserCredentials(string password, string username, out int userId);
    }
}