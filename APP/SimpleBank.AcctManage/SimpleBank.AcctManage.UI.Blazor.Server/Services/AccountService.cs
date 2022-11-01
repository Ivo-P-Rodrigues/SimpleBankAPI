using SimpleBank.AcctManage.UI.Blazor.Server.Services.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Responses;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Services
{
    public class AccountService
    {
        private readonly IHttpClientFactory _client;
        private readonly string _requestUri = "/api/v1/accounts/";

        public AccountService(
            IHttpClientFactory client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }


        public async Task<IEnumerable<AccountResponse>?> GetAllAccounts()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _requestUri);
            var client = _client.CreateClient("SbApi");

            var response = await client.SendAsync(request);
            if(!response.IsSuccessStatusCode) { return null; }

            return await response.Content.ReadFromJsonAsync<IEnumerable<AccountResponse>>();
        }

        public async Task<AccountMovims?> GetAccount(Guid accountId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _requestUri + accountId.ToString());
            var client = _client.CreateClient("SbApi");

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode) { return null; }

            return await response.Content.ReadFromJsonAsync<AccountMovims>();
        }

        public async Task<AccountResponse?> CreateAccount(CreateAccountRequest accountRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _requestUri);
            var client = _client.CreateClient("SbApi");

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode) { return null; }

            return await response.Content.ReadFromJsonAsync<AccountResponse>();
        }



    }
}
