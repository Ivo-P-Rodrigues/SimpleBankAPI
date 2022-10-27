using SimpleBank.AccountManage.UI.Blazor.Server.v2.Data.Requests;
using SimpleBank.AccountManage.UI.Blazor.Server.v2.Data.Responses;
using SimpleBank.AccountManage.UI.Blazor.Server.v2.Services.ApiConnect;

namespace SimpleBank.BlazorServerApp.Services
{
    public class UserService
    {
        private readonly SbApiConnect _sbApiConnect;
        private readonly SbLocalStorage _sbLocalStorage;
        private readonly string _requestUri = "/api/v1/users/";

        public UserService(
            SbApiConnect sbApiConnect,
            SbLocalStorage sbLocalStorage)
        {
            _sbApiConnect = sbApiConnect ?? throw new ArgumentNullException(nameof(sbApiConnect));
            _sbLocalStorage = sbLocalStorage ?? throw new ArgumentNullException(nameof(sbLocalStorage));
        }


        public async Task<CreateUserResponse?> Create(CreateUserRequest createUserRequest)
        {
            var httpRsp = await _sbApiConnect.PostAsync(_requestUri, createUserRequest);
            if (httpRsp == null) { return null; }

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(CreateUserResponse)) as CreateUserResponse;
            }
            return null;
        }




    }
}
