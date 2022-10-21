using SimpleBank.BlazorServerApp.Contracts;
using SimpleBank.BlazorServerApp.Data.Requests;
using SimpleBank.BlazorServerApp.Data.Responses;
using SimpleBank.BlazorServerApp.Services.Base;
using System.Net.Http.Headers;

namespace SimpleBank.BlazorServerApp.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly string _requestUri = "/api/accounts/";

        public AccountService(HttpClient httpClient, IUserStorage userStorage) : base(httpClient, userStorage)
        {
        }


        public async Task<IEnumerable<AccountResponse>?> GetAllAccounts(string accessToken)
        {
            var httpRsp = await GetAsync(_requestUri, true);
            if(httpRsp == null) { return null; }

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(IEnumerable<AccountResponse>)) as IEnumerable<AccountResponse>;
            }
            return null;
        }

        public async Task<AccountMovims?> GetAccount(Guid accountId, string accessToken)
        {
            var httpRsp = await GetAsync(_requestUri + accountId.ToString(), true);
            if (httpRsp == null) { return null; }

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(AccountMovims)) as AccountMovims;
            }
            return null;
        }

        public async Task<AccountResponse?> CreateAccount(CreateAccountRequest accountRequest, string accessToken)
        {
            var httpRsp = await PostAsync(_requestUri, accountRequest, true);
            if (httpRsp == null) { return null; }

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(AccountResponse)) as AccountResponse;
            }
            return null;
        }



    }
}
