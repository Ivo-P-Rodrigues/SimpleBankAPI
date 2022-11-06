using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SimpleBank.AcctManage.UI.Blazor.Server.Data;
using SimpleBank.AcctManage.UI.Blazor.Server.Providers;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Mapper;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Responses;
using System.Net.Http.Headers;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Services.HttpClients
{
    public class AuthClient
    {
        private readonly HttpClient _client;
        private readonly ProtectedLocalStorage _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly EntityMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthClient> _logger;

        private readonly string _tokenIdKey;
        private readonly string _accessTokenKey;
        private readonly string _refreshTokenKey;

        public AuthClient(
            HttpClient client,
            ProtectedLocalStorage localStorage,
            AuthenticationStateProvider authenticationStateProvider,
            EntityMapper mapper,
            IConfiguration configuration,
            ILogger<AuthClient> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
            _authenticationStateProvider = authenticationStateProvider ?? throw new ArgumentNullException(nameof(authenticationStateProvider));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _tokenIdKey = _configuration["StorageCustomKeys:tokenId"] ?? throw new ArgumentNullException("Invalid storage key: tokenId");
            _accessTokenKey = _configuration["StorageCustomKeys:accessToken"] ?? throw new ArgumentNullException("Invalid storage key: accessToken");
            _refreshTokenKey = _configuration["StorageCustomKeys:refreshToken"] ?? throw new ArgumentNullException("Invalid storage key: refreshToken");
        }


        public async Task<bool> Login(UserLogin userLogin)
        {
            RequestAuthLogin requestAuthLogin = _mapper.Map(userLogin);

            var request = new HttpRequestMessage(HttpMethod.Post, "login");
            request.Content = JsonContent.Create(requestAuthLogin);

            var response = await _client.SendAsync(request);
            if (response == null || !response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Login failed. Status code returned: {response?.StatusCode}");
                return false;
            }

            var content = await response.Content.ReadFromJsonAsync(typeof(ResponseAuthLogin)) as ResponseAuthLogin;
            if (content == null)
            {
                _logger.LogWarning($"Login failed. Error on parsing response to json");
                return false;
            }

            await _localStorage.SetAsync(_refreshTokenKey, content!.RefreshToken);
            await _localStorage.SetAsync(_accessTokenKey, content.AccessToken);
            await _localStorage.SetAsync(_tokenIdKey, content.UserTokenId);

            await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedIn();
            return true;
        }

        public async Task<bool> Logout()
        {
            Guid tokenId;
            var stringedTokenId = (await _localStorage.GetAsync<string>(_tokenIdKey)).Value;

            if (string.IsNullOrWhiteSpace(stringedTokenId))
            {
                _logger.LogCritical("Logout failed. Token Id not found.");
                return false;
            }
            if (!Guid.TryParse(stringedTokenId, out tokenId))
            {
                _logger.LogCritical("Logout failed. Token Id not valid.");
                return false;
            }

            RequestAuthLogout logoutUserRequest = new() { UserTokenId = tokenId };

            var request = new HttpRequestMessage(HttpMethod.Post, "logout");
            request.Content = JsonContent.Create(logoutUserRequest);


            var accessToken = (await _localStorage.GetAsync<string>(_accessTokenKey)).Value;
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                _logger.LogCritical("GetAllAccounts failed. Access token not found.");
                return false;
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

            var response = await _client.SendAsync(request);
            if (response == null || !response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Logout failed. Status code returned: {response?.StatusCode}");
                return false;
            }

            await _localStorage.DeleteAsync(_refreshTokenKey);
            await _localStorage.DeleteAsync(_accessTokenKey);
            await _localStorage.DeleteAsync(_tokenIdKey);

            ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOut();
            return true;
        }

        public async Task<object?> RefreshAsync()
        {
            var refreshToken = (await _localStorage.GetAsync<string>(_tokenIdKey)).Value;
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                _logger.LogCritical("Refresh failed. Refresh token not found.");
                return false;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, "renew");
            request.Content = JsonContent.Create(new RequestAuthRefresh { RefreshToken = refreshToken });

            var response = await _client.SendAsync(request);
            if (response == null || !response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Refresh failed. Status code returned: {response?.StatusCode}");
                return false;
            }

            var content = await response.Content.ReadFromJsonAsync(typeof(ResponseAuthLogin)) as ResponseAuthLogin;
            if (content == null)
            {
                _logger.LogWarning($"Refresh failed. Error on parsing response to json");
            }

            await _localStorage.SetAsync(_refreshTokenKey, content!.RefreshToken);
            await _localStorage.SetAsync(_accessTokenKey, content.AccessToken);
            await _localStorage.SetAsync(_tokenIdKey, content.UserTokenId);

            return content;
        }


    }
}
