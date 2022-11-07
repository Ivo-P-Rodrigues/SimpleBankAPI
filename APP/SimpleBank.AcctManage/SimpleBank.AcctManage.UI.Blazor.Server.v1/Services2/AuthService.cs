using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect;
using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect.Models.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect.Models.Responses;

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


        public async Task<bool> Login(LoginUserRequest loginUserRequest)
        {
            var response = await _sbApiConnect.PostAndStoreAsync<LoginUserResponse>(_requestUri + "login", loginUserRequest);
            if (!response)
            {
                response = await Refresh(); //this should be removed, if a user returns to the login screen, then it's probably because he has no authtoken in local storage
            }
            return response;
        }



        public async Task<bool> Logout() =>
            await _sbApiConnect.PostStoredAndUnregisterAsync<LogoutUserRequest, LoginUserResponse>(_requestUri + "logout", true);

        public async Task<bool> Refresh() =>
            await _sbApiConnect.PostStoredAndStoreAsync<RenewRequest, LoginUserResponse>(_requestUri + "renew", true);



    }
}


