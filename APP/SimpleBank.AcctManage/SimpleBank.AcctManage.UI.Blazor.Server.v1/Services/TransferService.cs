using SimpleBank.AcctManage.UI.Blazor.Server.v1.Contracts;
using SimpleBank.AcctManage.UI.Blazor.Server.v1.Data.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.v1.Data.Responses;
using SimpleBank.AcctManage.UI.Blazor.Server.v1.Services.Base;

namespace SimpleBank.AcctManage.UI.Blazor.Server.v1.Services
{
    public class TransferService : BaseService, ITransferService
    {
        private readonly string _requestUri;

        public TransferService(HttpClient httpClient, IUserStorage userStorage, IConfiguration configuration) : base(httpClient, userStorage, configuration)
        {
            _requestUri = Configuration["SbApiEndPointsAddresses:Transfers"];
        }


        public async Task<TransferResponse?> MakeTransfer(TransferRequest transferRequest)
        {
            var httpRsp = await PostAsync(_requestUri, transferRequest, true);
            if (httpRsp == null) { return null; }

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(TransferResponse)) as TransferResponse;
            }
            return null;
        }






    }
}
