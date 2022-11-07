using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect.Models.Responses;
using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect.Models.Requests;

namespace SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect
{
    public class RefreshHandler
    {
        private readonly HttpClient _client;
        private readonly string _requestUri = "/api/v1/auth/renew";

        public RefreshHandler(HttpClient client)
        {
            _client = client;
        }

        public async Task<object?> RefreshAsync(string refreshToken)
        {
            var renewObj = new RenewRequest { RefreshToken = refreshToken };
            var response = await _client.PostAsJsonAsync(_requestUri, renewObj);
            if (!response.IsSuccessStatusCode || response == null) { return null; }

            var content = await response.Content.ReadFromJsonAsync(typeof(LoginUserResponse)) as LoginUserResponse;

            return content;
        }

    }
}

/*
     public class RefreshHandler
    {
        private readonly IHttpClientFactory _client;
        private readonly string _requestUri = "/api/v1/auth/renew";

        public RefreshHandler(IHttpClientFactory client)
        {
            _client = client;
        }

        public async Task<object?> RefreshAsync(string refreshToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _requestUri);
            var renewObj = JsonSerializer.Serialize(new RenewRequest { RefreshToken = refreshToken });
            request.Content = new StringContent(renewObj, Encoding.UTF8, "application/json");

            var client = _client.CreateClient();

            var response = await client.SendAsync(request);
            //var response = await client.PostAsJsonAsync(_requestUri, renewObj);
            if (!response.IsSuccessStatusCode || response == null) { return null; }

            //var content = await response.Content.ReadAsStringAsync();
            //var token = JsonSerializer.Deserialize<RenewRequest>(content);

            var content = await response.Content.ReadFromJsonAsync(typeof(LoginUserResponse)) as LoginUserResponse;

            return content;
        }

    }
 */







/*

        private readonly HttpClient _httpClient;
        private readonly string _requestUri = "/api/v1/auth/"; 

        public async Task<LoginUserResponse?> RenewToken(RenewRequest renewRequest)
        {
            var httpRsp = await _httpClient.PostAsJsonAsync(_requestUri + "renew", renewRequest);

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(LoginUserResponse)) as LoginUserResponse;
            }
            return null;
        }
        //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

***************************************************************************************************************

    public class RefreshService
    {
        private readonly SbApiConnect _sbApiConnect;
        private readonly string _requestUri = "/api/v1/auth/renew";

        public RefreshService(SbApiConnect sbApiConnect)
        {
            _sbApiConnect = sbApiConnect ?? throw new ArgumentNullException(nameof(sbApiConnect));
        }

        public async Task<bool> RefreshAsync() =>
            await _sbApiConnect.PostStoredAndStoreAsync<RenewRequest, LoginUserResponse>(_requestUri);
    }
*/