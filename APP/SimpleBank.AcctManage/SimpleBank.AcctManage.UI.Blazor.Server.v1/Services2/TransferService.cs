using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect;
using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect.Models.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect.Models.Responses;

namespace SimpleBank.AcctManage.UI.Blazor.Server.v2.Services
{
    public class TransferService
    {
        private readonly SbApiConnect _sbApiConnect;
        private readonly string _requestUri = "/api/v1/transfers/";

        public TransferService(SbApiConnect sbApiConnect)
        {
            _sbApiConnect = sbApiConnect ?? throw new ArgumentNullException(nameof(sbApiConnect));
        }


        public async Task<TransferResponse?> MakeTransfer(TransferRequest transferRequest) =>
            await _sbApiConnect.PostAndReturnAsync<TransferResponse>(_requestUri, transferRequest, true) as TransferResponse;





    }
}
