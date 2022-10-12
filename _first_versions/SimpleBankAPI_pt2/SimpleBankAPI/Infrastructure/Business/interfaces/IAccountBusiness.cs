using SimpleBankAPI.Contracts;
using SimpleBankAPI.Core.Models;

namespace SimpleBankAPI.Business
{
    public interface IAccountBusiness
    {
        Task<Account?> CreateAccount(Account account);
        Task<(bool, Account?, IEnumerable<Movement>?)> GetAccountWithMovements(int accountId, int userId);
        IEnumerable<Account> GetAllUserAccounts(int userId);

    }
}