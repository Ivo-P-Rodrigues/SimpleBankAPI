using SimpleBank.AccountManage.UI.Blazor.Server.v1.Contracts;
using SimpleBank.AccountManage.UI.Blazor.Server.v1.Data.Requests;
using SimpleBank.AccountManage.UI.Blazor.Server.v1.Services.Base;

namespace SimpleBank.AccountManage.UI.Blazor.Server.v1.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly string _requestUri;

        public AuthService(HttpClient httpClient, IUserStorage userStorage, IConfiguration configuration) : base(httpClient, userStorage, configuration)
        {
            _requestUri = Configuration["SbApiEndPointsAddresses:Auth"];
        }


        public async Task<bool> Login(LoginUserRequest loginUserRequest) =>
            await RegisterLogin(loginUserRequest);
        public async Task<bool> Logout() =>
            await RegisterLogoutAsync();




    }
}



