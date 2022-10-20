using SimpleBank.BlazorServerApp.Data.Requests;
using SimpleBank.BlazorServerApp.Data.Responses;

namespace SimpleBank.BlazorServerApp.Contracts
{
    public interface IAccountService
    {
        Task<AccountResponse?> CreateAccount(CreateAccountRequest accountRequest, string accessToken);
        Task<AccountMovims?> GetAccount(Guid accountId, string accessToken);
        Task<IEnumerable<AccountResponse>?> GetAllAccounts(string accessToken);
    }
}