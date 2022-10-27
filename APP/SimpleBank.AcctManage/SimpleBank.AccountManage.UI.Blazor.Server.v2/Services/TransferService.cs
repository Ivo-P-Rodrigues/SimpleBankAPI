using SimpleBank.AccountManage.UI.Blazor.Server.v2.Data.Requests;
using SimpleBank.AccountManage.UI.Blazor.Server.v2.Data.Responses;
using SimpleBank.AccountManage.UI.Blazor.Server.v2.Services.ApiConnect;

namespace SimpleBank.BlazorServerApp.Services
{
    public class TransferService
    {
        private readonly SbApiConnect _sbApiConnect;
        private readonly SbLocalStorage _sbLocalStorage;
        private readonly string _requestUri = "/api/v1/transfers/";

        public TransferService(
            SbApiConnect sbApiConnect,
            SbLocalStorage sbLocalStorage)
        {
            _sbApiConnect = sbApiConnect ?? throw new ArgumentNullException(nameof(sbApiConnect));
            _sbLocalStorage = sbLocalStorage ?? throw new ArgumentNullException(nameof(sbLocalStorage));
        }


        public async Task<TransferResponse?> MakeTransfer(TransferRequest transferRequest)
        {
            var accessToken = await _sbLocalStorage.GetAsync("AccessToken");
            if (accessToken == null) { return null; }

            var httpRsp = await _sbApiConnect.PostAsync(_requestUri, transferRequest, accessToken);
            if (httpRsp == null) { return null; }

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(TransferResponse)) as TransferResponse;
            }
            return null;
        }






    }
}
