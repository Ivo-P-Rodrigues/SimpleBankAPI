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
        private readonly string _requestUri;

        public TransferService(HttpClient httpClient, IUserStorage userStorage, IConfiguration configuration) :base(httpClient, userStorage, configuration)
        {
            _requestUri = Configuration["SbApiEndPointsAddresses:Transfers"];
        }


        public async Task<TransferResponse?> MakeTransfer(TransferRequest transferRequest)
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
