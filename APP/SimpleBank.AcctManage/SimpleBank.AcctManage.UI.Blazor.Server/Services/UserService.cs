using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Responses;
using System.Net.Http.Headers;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Services
{
    public class UserService
    {
        private readonly IHttpClientFactory _client;
        private readonly ProtectedLocalStorage _localStorage;
        private readonly ILogger<UserService> _logger;
        private readonly string _requestUri = "/api/v1/users/";

        public UserService(
            IHttpClientFactory client,
            ProtectedLocalStorage localStorage,
            ILogger<UserService> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task<bool> Create(CreateUserRequest createUserRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _requestUri);
            request.Content = JsonContent.Create(createUserRequest);

            var client = _client.CreateClient("SbApi");
            var response = await client.SendAsync(request);

            if (response == null || !response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Create user failed. Status code returned: {response?.StatusCode}");
            }

            return response!.IsSuccessStatusCode;
        }


        public async Task<CreateUserResponse?> GetUser()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _requestUri + "profile");

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
                _logger.LogWarning($"GetUser user failed. Status code returned: {response?.StatusCode}");
                return null;
            }
            return await response.Content.ReadFromJsonAsync(typeof(CreateUserResponse)) as CreateUserResponse;
        }





    }
}
