using SimpleBank.AcctManage.UI.Blazor.Server.v1.Contracts;
using SimpleBank.AcctManage.UI.Blazor.Server.v1.Data.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.v1.Data.Responses;
using SimpleBank.AcctManage.UI.Blazor.Server.v1.Services.Base;

namespace SimpleBank.AcctManage.UI.Blazor.Server.v1.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private string _requestUri;
        public AccountService(HttpClient httpClient, IUserStorage userStorage, IConfiguration configuration) : base(httpClient, userStorage, configuration)
        {
            _requestUri = Configuration["SbApiEndPointsAddresses:Accounts"];
        }

        #region old - first api calls
        public async Task<IEnumerable<AccountResponse>?> OldGetAllAccounts(string accessToken)
        {
            var httpRsp = await GetAsync(_requestUri, true);
            if (httpRsp == null) { return null; }

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(IEnumerable<AccountResponse>)) as IEnumerable<AccountResponse>;
            }
            return null;
        }

        public async Task<AccountMovims?> OldGetAccount(Guid accountId, string accessToken)
        {
            var httpRsp = await GetAsync(_requestUri + accountId.ToString(), true);
            if (httpRsp == null) { return null; }

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(AccountMovims)) as AccountMovims;
            }
            return null;
        }

        public async Task<AccountResponse?> OldCreateAccount(CreateAccountRequest accountRequest, string accessToken)
        {
            var httpRsp = await PostAsync(_requestUri, accountRequest, true);
            if (httpRsp == null) { return null; }

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(AccountResponse)) as AccountResponse;
            }
            return null;
        }
        #endregion


        public async Task<IEnumerable<AccountResponse>?> GetAllAccounts()
        {
            var httpRsp = await GetAsync(_requestUri, true);
            if (httpRsp == null) { return null; }

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(IEnumerable<AccountResponse>)) as IEnumerable<AccountResponse>;
            }
            return null;
        }

        public async Task<AccountMovims?> GetAccount(Guid accountId)
        {
            var httpRsp = await GetAsync(_requestUri + accountId.ToString(), true);
            if (httpRsp == null) { return null; }

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(AccountMovims)) as AccountMovims;
            }
            return null;
        }

        public async Task<AccountResponse?> CreateAccount(CreateAccountRequest accountRequest)
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
