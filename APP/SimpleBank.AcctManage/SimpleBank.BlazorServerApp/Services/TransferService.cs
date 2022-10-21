using SimpleBank.BlazorServerApp.Data.Requests;
using SimpleBank.BlazorServerApp.Data.Responses;
using System.Text.Json;
using System.Net.Http.Headers;
using SimpleBank.BlazorServerApp.Contracts;
using SimpleBank.BlazorServerApp.Services.Base;

namespace SimpleBank.BlazorServerApp.Services
{
    public class TransferService : BaseService, ITransferService
    {
        private readonly string _requestUri = "/api/transfers/";

        public TransferService(HttpClient httpClient, IUserStorage userStorage) :base(httpClient, userStorage)
        {
        }


        public async Task<TransferResponse?> MakeTransfer(TransferRequest transferRequest, string accessToken)
        {
            var httpRsp = await PostAsync(_requestUri, transferRequest, true);
            if(httpRsp == null) { return null; }

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(TransferResponse)) as TransferResponse;
            }
            return null;
        }






    }
}
