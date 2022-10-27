using SimpleBank.AccountManage.UI.Blazor.Server.v2.Data.Requests;
using SimpleBank.AccountManage.UI.Blazor.Server.v2.Data.Responses;
using SimpleBank.AccountManage.UI.Blazor.Server.v2.Services.ApiConnect;

namespace SimpleBank.BlazorServerApp.Services
{
    public class AccountService
    {
        private readonly SbApiConnect _sbApiConnect;
        private readonly SbLocalStorage _sbLocalStorage;
        private readonly string _requestUri = "/api/v1/accounts/";

        public AccountService(
            SbApiConnect sbApiConnect,
            SbLocalStorage sbLocalStorage)
        {
            _sbApiConnect = sbApiConnect ?? throw new ArgumentNullException(nameof(sbApiConnect));
            _sbLocalStorage = sbLocalStorage ?? throw new ArgumentNullException(nameof(sbLocalStorage));
        }

        public async Task<IEnumerable<AccountResponse>?> GetAllAccounts()
        {
            var accessToken = await _sbLocalStorage.GetAsync("AccessToken");
            if (accessToken == null) { return null; }

            var httpRsp = await _sbApiConnect.GetAsync(_requestUri, accessToken);
            if (httpRsp == null) { return null; }

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(IEnumerable<AccountResponse>)) as IEnumerable<AccountResponse>;
            }
            return null;
        }

        public async Task<AccountMovims?> GetAccount(Guid accountId)
        {
            var accessToken = await _sbLocalStorage.GetAsync("AccessToken");
            if (accessToken == null) { return null; }

            var httpRsp = await _sbApiConnect.GetAsync(_requestUri + accountId.ToString(), accessToken);
            if (httpRsp == null) { return null; }

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(AccountMovims)) as AccountMovims;
            }
            return null;
        }

        public async Task<AccountResponse?> CreateAccount(CreateAccountRequest accountRequest)
        {
            var accessToken = await _sbLocalStorage.GetAsync("AccessToken");
            if (accessToken == null) { return null; }

            var httpRsp = await _sbApiConnect.PostAsync(_requestUri, accountRequest, accessToken);
            if (httpRsp == null) { return null; }

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(AccountResponse)) as AccountResponse;
            }
            return null;
        }


    }
}
