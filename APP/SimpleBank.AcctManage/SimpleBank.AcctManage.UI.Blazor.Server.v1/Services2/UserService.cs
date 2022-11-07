using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect;
using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect.Models.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect.Models.Responses;

namespace SimpleBank.AcctManage.UI.Blazor.Server.v2.Services
{
    public class UserService
    {
        private readonly SbApiConnect _sbApiConnect;
        private readonly string _requestUri = "/api/v1/users/";

        public UserService(SbApiConnect sbApiConnect)
        {
            _sbApiConnect = sbApiConnect ?? throw new ArgumentNullException(nameof(sbApiConnect));
        }


        public async Task<CreateUserResponse?> Create(CreateUserRequest createUserRequest) =>
            await _sbApiConnect.PostAndReturnAsync<CreateUserResponse>(_requestUri, createUserRequest, true) as CreateUserResponse;




    }
}
