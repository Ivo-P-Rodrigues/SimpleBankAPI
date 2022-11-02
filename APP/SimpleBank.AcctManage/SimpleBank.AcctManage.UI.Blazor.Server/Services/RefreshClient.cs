using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Responses;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Services
{
    public class RefreshClient
    {
        private readonly HttpClient _client;
        private readonly ProtectedLocalStorage _localStorage;
        private readonly ILogger<UserService> _logger;

        public RefreshClient(
            HttpClient client,
            ProtectedLocalStorage localStorage,
            ILogger<UserService> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<object?> RefreshAsync()
        {
            var refreshToken = (await _localStorage.GetAsync<string>("tokenId")).Value;
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                _logger.LogCritical("Refresh failed. Refresh token not found.");
                return false;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, (string?)null);
            request.Content = JsonContent.Create(new RenewRequest { RefreshToken = refreshToken });

            var response = await _client.SendAsync(request);
            if (response == null || !response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Refresh failed. Status code returned: {response?.StatusCode}");
                return false;
            }

            var content = await response.Content.ReadFromJsonAsync(typeof(LoginUserResponse)) as LoginUserResponse;
            if (content == null)
            {
                _logger.LogWarning($"Refresh failed. Error on parsing response to json");
            }
            return content;
        }






    }
}
