using SimpleBank.AcctManage.UI.Blazor.Server.v1.Contracts;
using SimpleBank.AcctManage.UI.Blazor.Server.v1.Data.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.v1.Data.Responses;
using SimpleBank.AcctManage.UI.Blazor.Server.v1.Services.Base;

namespace SimpleBank.AcctManage.UI.Blazor.Server.v1.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly string _requestUri;

        public UserService(HttpClient httpClient, IUserStorage userStorage, IConfiguration configuration) : base(httpClient, userStorage, configuration)
        {
            _requestUri = Configuration["SbApiEndPointsAddresses:Users"];
        }

        public async Task<CreateUserResponse?> Create(CreateUserRequest createUserRequest)
        {
            var httpRsp = await PostAsync(_requestUri, createUserRequest);
            if (httpRsp == null) { return null; }

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(CreateUserResponse)) as CreateUserResponse;
            }
            return null;
        }


        public async Task<CreateUserResponse?> GetUser()
        {
            var httpRsp = await GetAsync(_requestUri + "profile", true);
            if (httpRsp == null) { return null; }

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(CreateUserResponse)) as CreateUserResponse;
            }
            return null;
        }

    }
}



