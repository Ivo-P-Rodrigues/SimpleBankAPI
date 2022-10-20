using SimpleBank.BlazorServerApp.Data.Requests;
using SimpleBank.BlazorServerApp.Data.Responses;
using System.Text.Json;
using System.Net.Http.Headers;
using SimpleBank.BlazorServerApp.Contracts;

namespace SimpleBank.BlazorServerApp.Services
{
    public class TransferService : ITransferService
    {
        private readonly HttpClient _httpClient;
        private readonly string _requestUri = "/api/transfers/";

        public TransferService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }


        public async Task<TransferResponse?> MakeTransfer(TransferRequest transferRequest, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var httpRsp = await _httpClient.PostAsJsonAsync(_requestUri, transferRequest);
            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(TransferResponse)) as TransferResponse;
            }
            return null;
        }






    }
}
