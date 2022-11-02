using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SimpleBank.AcctManage.UI.Blazor.Server.Providers;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Responses;
using System.Net.Http.Headers;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Services
{
    public class AuthService
    {
        private readonly IHttpClientFactory _client;
        private readonly ILogger<UserService> _logger;
        private readonly ProtectedLocalStorage _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly string _requestUri = "/api/v1/auth/";

        public AuthService(
            IHttpClientFactory client,
            ILogger<UserService> logger,
            ProtectedLocalStorage localStorage,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
            _authenticationStateProvider = authenticationStateProvider ?? throw new ArgumentNullException(nameof(authenticationStateProvider));
        }

        public async Task<bool> Login(LoginUserRequest loginUserRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _requestUri + "login");
            request.Content = JsonContent.Create(loginUserRequest);

            var client = _client.CreateClient("SbApi");
            var response = await client.SendAsync(request);
            if (response == null || !response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Login failed. Status code returned: {response?.StatusCode}");
                return false;
            }

            var content = await response.Content.ReadFromJsonAsync(typeof(LoginUserResponse)) as LoginUserResponse;
            if (content == null)
            {
                _logger.LogWarning($"Login failed. Error on parsing response to json");
                return false;
            }

            await _localStorage.SetAsync("tokenId", content.UserTokenId);
            await _localStorage.SetAsync("accessToken", content.AccessToken);
            await _localStorage.SetAsync("refreshToken", content.RefreshToken);

            await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedIn();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", content.AccessToken);
            return true;
        }

        public async Task<bool> Logout()
        {
            Guid tokenId;
            var stringedTokenId = (await _localStorage.GetAsync<string>("tokenId")).Value;

            if (string.IsNullOrWhiteSpace(stringedTokenId))
            {
                _logger.LogCritical("Logout failed. Token Id not found.");
                return false;
            }
            if(!Guid.TryParse(stringedTokenId, out tokenId))
            {
                _logger.LogCritical("Logout failed. Token Id not valid.");
                return false;
            }

            LogoutUserRequest logoutUserRequest = new() { UserTokenId = tokenId };

            var request = new HttpRequestMessage(HttpMethod.Post, _requestUri + "logout");
            request.Content = JsonContent.Create(logoutUserRequest);

            var client = _client.CreateClient("SbApi");

            var accessToken = (await _localStorage.GetAsync<string>("accessToken")).Value;
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                _logger.LogCritical("GetAllAccounts failed. Access token not found.");
                return false;
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

            var response = await client.SendAsync(request);
            if (response == null || !response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Logout failed. Status code returned: {response?.StatusCode}");
                return false;
            }

            await _localStorage.DeleteAsync("tokenId");
            await _localStorage.DeleteAsync("accessToken");
            await _localStorage.DeleteAsync("refreshToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOut();
            return true;
        }





    }
}
