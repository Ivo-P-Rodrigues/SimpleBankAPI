using SimpleBank.AccountManage.UI.Blazor.Server.v1.Data.Requests;
using SimpleBank.AccountManage.UI.Blazor.Server.v1.Data.Responses;

namespace SimpleBank.AccountManage.UI.Blazor.Server.v1.Contracts
{
    public interface IAccountService
    {
        Task<AccountResponse?> CreateAccount(CreateAccountRequest accountRequest);
        Task<AccountMovims?> GetAccount(Guid accountId);
        Task<IEnumerable<AccountResponse>?> GetAllAccounts();

        Task<bool> CheckLocallyIfUserIsLoggedAsync();
    }
}