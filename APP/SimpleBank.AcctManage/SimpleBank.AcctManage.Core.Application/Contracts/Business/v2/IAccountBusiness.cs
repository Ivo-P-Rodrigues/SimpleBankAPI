using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Business.v2
{
    public interface IAccountBusiness
    {
        IEnumerable<Account> GetAll(Guid userId);
        Account? Get(Guid accountId);
        Task<Account?> CreateAsync(Account account);

        Task<bool> CheckIfUserOwnsAccountAsync(Guid accountId, Guid userId);






    }
}