using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect;
using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect.Responses;

namespace SimpleBank.AcctManage.UI.Blazor.Server.v2.Services
{
    public class AuthService
    {
        private readonly SbApiConnect _sbApiConnect;
        private readonly string _requestUri = "/api/v1/auth/";

        public AuthService(SbApiConnect sbApiConnect)
        {
            _sbApiConnect = sbApiConnect ?? throw new ArgumentNullException(nameof(sbApiConnect));
        }


        public async Task<bool> Login(LoginUserRequest loginUserRequest) =>
            await _sbApiConnect.PostAndStoreAsync<LoginUserResponse>(_requestUri + "login", loginUserRequest);

        public async Task<bool> Logout() =>
            await _sbApiConnect.PostStoredAndUnregisterAsync<LogoutUserRequest, LoginUserResponse>(_requestUri + "logout", true);
       
        public async Task<bool> Refresh() =>
            await _sbApiConnect.PostStoredAndStoreAsync<RenewRequest, LoginUserResponse>(_requestUri + "renew", true);
    


    }
}


