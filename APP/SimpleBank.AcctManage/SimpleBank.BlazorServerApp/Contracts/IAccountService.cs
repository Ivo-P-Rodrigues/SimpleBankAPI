using SimpleBank.BlazorServerApp.Data.Requests;
using SimpleBank.BlazorServerApp.Data.Responses;

namespace SimpleBank.BlazorServerApp.Contracts
{
    public interface IAccountService
    {
        Task<AccountResponse?> CreateAccount(CreateAccountRequest accountRequest);
        Task<AccountMovims?> GetAccount(Guid accountId);
        Task<IEnumerable<AccountResponse>?> GetAllAccounts();

        Task<bool> CheckLocallyIfUserIsLoggedAsync();
    }
}