using SimpleBank.AcctManage.UI.Blazor.Server.Services.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Responses;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Services
{
    public class RefreshClient
    {
        private readonly HttpClient _client;

        public RefreshClient(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<object?> RefreshAsync(string refreshToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, (string?)null);
            request.Content = JsonContent.Create(new RenewRequest { RefreshToken = refreshToken });

            var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode || response == null) { return null; }

            var content = await response.Content.ReadFromJsonAsync(typeof(LoginUserResponse)) as LoginUserResponse;
            return content;
        }






    }
}
