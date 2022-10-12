using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Business
{
    public interface IAccountBusiness
    {
        Task<Account?> CreateAccount(Account account);
        Task<(bool, Account?, IEnumerable<Movement>?)> GetAccountWithMovements(Guid accountId, Guid userId);
        IEnumerable<Account> GetAllUserAccounts(Guid userId);
    }
}