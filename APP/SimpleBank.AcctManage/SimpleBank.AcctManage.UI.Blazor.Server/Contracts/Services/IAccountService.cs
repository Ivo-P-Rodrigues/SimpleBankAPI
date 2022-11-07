using SimpleBank.AcctManage.UI.Blazor.Server.Data;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Contracts.Services
{
    public interface IAccountService
    {
        Task<Account?> Create(AccountCreate accountCreate);
        Task<Account?> Get(Guid accountId);
        Task<IEnumerable<Account>?> GetAll();
    }
}