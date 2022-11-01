using SimpleBank.AcctManage.UI.Blazor.Server.Services.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.Services.Responses;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Services
{
    public class TransferService
    {
        private readonly IHttpClientFactory _client;
        private readonly string _requestUri = "/api/v1/transfers/";

        public TransferService(IHttpClientFactory client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }
        public async Task<TransferResponse?> MakeTransfer(TransferRequest transferRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _requestUri);
            var client = _client.CreateClient("SbApi");

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode) { return null; }

            return await response.Content.ReadFromJsonAsync<TransferResponse>();
        }




    }
}
