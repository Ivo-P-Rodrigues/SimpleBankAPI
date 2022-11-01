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
        private readonly ProtectedLocalStorage _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly string _requestUri = "/api/v1/auth/";

        public AuthService(
            IHttpClientFactory client,
            ProtectedLocalStorage localStorage,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
            _authenticationStateProvider = authenticationStateProvider ?? throw new ArgumentNullException(nameof(authenticationStateProvider));
        }

        public async Task<bool> Login(LoginUserRequest loginUserRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _requestUri + "login");
            request.Content = JsonContent.Create(loginUserRequest);

            var client = _client.CreateClient("SbApi");
            var response = await client.SendAsync(request);
            if (response == null || !response.IsSuccessStatusCode) { return false; }

            var content = await response.Content.ReadFromJsonAsync(typeof(LoginUserResponse)) as LoginUserResponse;
            if (content == null) { return false; }

            await _localStorage.SetAsync("authToken", content.AccessToken);
            await _localStorage.SetAsync("refreshToken", content.RefreshToken);

            await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedIn();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", content.AccessToken);
            return true;
        }

        public async Task<bool> Logout(LogoutUserRequest logoutUserRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _requestUri + "logout");
            request.Content = JsonContent.Create(logoutUserRequest);

            var client = _client.CreateClient("SbApi");
            var response = await client.SendAsync(request);
            if (response == null || !response.IsSuccessStatusCode) { return false; }

            await _localStorage.DeleteAsync("authToken");
            await _localStorage.DeleteAsync("refreshToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOut();
            return true;
        }





    }
}
