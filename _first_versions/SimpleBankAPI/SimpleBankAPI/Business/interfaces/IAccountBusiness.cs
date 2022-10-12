using SimpleBankAPI.Contracts;
using SimpleBankAPI.Models;

namespace SimpleBankAPI.Business
{
    public interface IAccountBusiness
    {
        Task<Contracts.AccountResponse?> CreateAccount(CreateAccountRequest accountRequest, int userId);
        Task<AccountMovims?> GetAccount(int accountId);
        IEnumerable<Contracts.AccountResponse> GetAllUserAccounts(int userId);
        Task<IEnumerable<Contracts.AccountResponse>> GetAllUserAccountsAsync(int userId);
        bool CheckUserOwnsAccount(int userId, int accountId);
    }
}