using SimpleBank.BlazorServerApp.Data.Requests;
using SimpleBank.BlazorServerApp.Data.Responses;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SimpleBank.BlazorServerApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly string _requestUri = "/api/accounts/";

        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }


        public async Task<IEnumerable<AccountResponse>?> GetAllAccounts(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var httpRsp = await _httpClient.PostAsJsonAsync(_requestUri, "");
            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(IEnumerable<AccountResponse>)) as IEnumerable<AccountResponse>;
            }
            return null;
        }

        public async Task<AccountMovims?> GetAccount(Guid accountId, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var httpRsp = await _httpClient.PostAsync(_requestUri + accountId.ToString(), null);
            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(AccountMovims)) as AccountMovims;
            }
            return null;
        }

        public async Task<AccountResponse?> CreateAccount(CreateAccountRequest accountRequest, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var httpRsp = await _httpClient.PostAsJsonAsync(_requestUri, accountRequest);
            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(AccountResponse)) as AccountResponse;
            }
            return null;
        }



    }
}
