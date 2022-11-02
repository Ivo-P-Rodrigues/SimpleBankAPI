using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Responses;
using System.Net.Http.Headers;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Services
{
    public class AccountService
    {
        private readonly IHttpClientFactory _client;
        private readonly ProtectedLocalStorage _localStorage;
        private readonly ILogger<UserService> _logger;
        private readonly string _requestUri = "/api/v1/accounts/";

        public AccountService(
            IHttpClientFactory client,
            ProtectedLocalStorage protectedLocalStorage,
            ILogger<UserService> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _localStorage = protectedLocalStorage ?? throw new ArgumentNullException(nameof(protectedLocalStorage));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task<IEnumerable<AccountResponse>?> GetAllAccounts()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _requestUri);
            var client = _client.CreateClient("SbApi");

            var accessToken = (await _localStorage.GetAsync<string>("accessToken")).Value;
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                _logger.LogCritical("GetAllAccounts failed. Access token not found.");
                return null;
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

            var response = await client.SendAsync(request);
            if (response == null || !response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"GetAllAccounts failed. Status code returned: {response?.StatusCode}");
                return null;
            }

            var content = await response.Content.ReadFromJsonAsync<IEnumerable<AccountResponse>>();
            if (content == null)
            {
                _logger.LogWarning($"GetAllAccounts failed. Error on parsing response to json");
            }
            return content;
        }

        public async Task<AccountMovims?> GetAccount(Guid accountId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _requestUri + accountId.ToString());
            var client = _client.CreateClient("SbApi");

            var accessToken = (await _localStorage.GetAsync<string>("accessToken")).Value;
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                _logger.LogCritical("GetAllAccounts failed. Access token not found.");
                return null;
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

            var response = await client.SendAsync(request);
            if (response == null || !response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"GetAccount failed. Status code returned: {response?.StatusCode}");
                return null;
            }

            var content = await response.Content.ReadFromJsonAsync<AccountMovims>();
            {
                _logger.LogWarning($"GetAccount failed. Error on parsing response to json");
            }
            return content;
        }

        public async Task<AccountResponse?> CreateAccount(CreateAccountRequest accountRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _requestUri);
            var client = _client.CreateClient("SbApi");

            var accessToken = (await _localStorage.GetAsync<string>("accessToken")).Value;
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                _logger.LogCritical("GetAllAccounts failed. Access token not found.");
                return null;
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

            var response = await client.SendAsync(request);
            if (response == null || !response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"CreateAccount failed. Status code returned: {response?.StatusCode}");
                return null;
            }

            var content = await response.Content.ReadFromJsonAsync<AccountResponse>();
            if (content == null)
            {
                _logger.LogWarning($"CreateAccount failed. Error on parsing response to json");
            }
            return content;
        }



    }
}
