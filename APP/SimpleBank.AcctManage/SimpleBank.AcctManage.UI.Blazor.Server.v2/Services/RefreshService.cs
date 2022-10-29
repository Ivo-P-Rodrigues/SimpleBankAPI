using Microsoft.AspNetCore.Components;
using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect;
using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect.Responses;

namespace SimpleBank.AcctManage.UI.Blazor.Server.v2.Services
{
    public class RefreshService
    {
        private readonly SbApiConnect _sbApiConnect;
        private readonly string _requestUri = "/api/v1/auth/renew";

        public RefreshService(SbApiConnect sbApiConnect)
        {
            _sbApiConnect = sbApiConnect ?? throw new ArgumentNullException(nameof(sbApiConnect));
        }

        public async Task<bool> Refresh() =>
            await _sbApiConnect.PostStoredAndStoreAsync<RenewRequest, LoginUserResponse>(_requestUri); 
          


        public static EventCallback<TransferResponse> OnTransferUpdateBalance { get; set; }






    }
}
