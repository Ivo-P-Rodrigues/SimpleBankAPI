using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Responses;
using System.Net.Http.Headers;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Services
{
    public class TransferService
    {
        private readonly IHttpClientFactory _client;
        private readonly ProtectedLocalStorage _localStorage;
        private readonly ILogger<TransferService> _logger;
        private readonly string _requestUri = "/api/v1/transfers/";


        public TransferService(
            IHttpClientFactory client,
            ProtectedLocalStorage localStorage,
            ILogger<TransferService> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task<TransferResponse?> MakeTransfer(TransferRequest transferRequest)
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
                _logger.LogWarning($"Transfer not executed. Status code returned: {response?.StatusCode}");
                return null;
            }

            return await response.Content.ReadFromJsonAsync<TransferResponse>();
        }




    }
}
