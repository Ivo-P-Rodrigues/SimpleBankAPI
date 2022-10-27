using SimpleBank.AccountManage.UI.Blazor.Server.v2.Data.Requests;
using SimpleBank.AccountManage.UI.Blazor.Server.v2.Data.Responses;
using SimpleBank.AccountManage.UI.Blazor.Server.v2.Services.ApiConnect;

namespace SimpleBank.BlazorServerApp.Services
{
    public class AuthService
    {
        private readonly SbApiConnect _sbApiConnect;
        private readonly SbLocalStorage _sbLocalStorage;
        private readonly string _requestUri = "/api/v1/auth/";

        public AuthService(
            SbApiConnect sbApiConnect,
            SbLocalStorage sbLocalStorage)            
        {
            _sbApiConnect = sbApiConnect ?? throw new ArgumentNullException(nameof(sbApiConnect));
            _sbLocalStorage = sbLocalStorage ?? throw new ArgumentNullException(nameof(sbLocalStorage));
        }


        public async Task<LoginUserResponse?> Login(LoginUserRequest loginUserRequest)
        {
            var httpRsp = await _sbApiConnect.PostAsync(_requestUri + "login", loginUserRequest);
            if (httpRsp == null) { return null; }

            if (httpRsp.IsSuccessStatusCode)
            {
                var loginResponse = await httpRsp.Content.ReadFromJsonAsync(typeof(LoginUserResponse)) as LoginUserResponse;
                if (loginResponse == null) { return null; }

                await _sbLocalStorage.SetObjAsync(loginResponse);
                return loginResponse;
            }
            return null;
        }

        public async Task<bool> Logout()
        {
            var userTokenId = await _sbLocalStorage.GetAsync("UserTokenId");
            var accessToken = await _sbLocalStorage.GetAsync("AccessToken");
            if(userTokenId == null || accessToken == null) { return false; }

            var logoutUserRequest = new LogoutUserRequest() { UserTokenId = Guid.Parse(userTokenId) };

            var httpRsp = await _sbApiConnect.PostAsync(_requestUri + "logout", logoutUserRequest, accessToken);
            if (httpRsp == null) { return false; }

            if (httpRsp.IsSuccessStatusCode)
            {
                //return await httpRsp.Content.ReadAsStringAsync();
                await _sbLocalStorage.DeleteObjAsync(new LoginUserResponse());
                return true;
            }
            return false;
        }

    

    }
}




//httpRsp.RequestMessage
//httpRsp.Content.Headers

//var custome‌​rJsonString = await httpRsp.Content.ReadAsStringAsync()
//JsonConvert.DeserializeObject<Customer>(custome‌​rJsonString)

//_httpClient.PostAsync()
//_httpClient.GetAsync()