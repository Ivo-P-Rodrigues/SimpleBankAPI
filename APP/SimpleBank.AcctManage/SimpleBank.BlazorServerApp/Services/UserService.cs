using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SimpleBank.BlazorServerApp.Data.Requests;
using SimpleBank.BlazorServerApp.Data.Responses;
using System.Net.Http.Headers;

namespace SimpleBank.BlazorServerApp.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly string _requestUri = "/api/users/";

        private readonly ProtectedLocalStorage _protectedLocalStorage;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }


        public async Task<CreateUserResponse?> Create(CreateUserRequest createUserRequest)
        {
            var httpRsp = await _httpClient.PostAsJsonAsync(_requestUri, createUserRequest);

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(CreateUserResponse)) as CreateUserResponse;
            }
            return null;
        }

        public async Task<LoginUserResponse?> Login(LoginUserRequest loginUserRequest)
        {
            var httpRsp = await _httpClient.PostAsJsonAsync(_requestUri + "login", loginUserRequest);

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(LoginUserResponse)) as LoginUserResponse;
            }
            return null;
        }


        public async Task<LoginUserResponse?> RenewToken(RenewRequest renewRequest)
        {
            var httpRsp = await _httpClient.PostAsJsonAsync(_requestUri + "renew", renewRequest);

            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(LoginUserResponse)) as LoginUserResponse;
            }
            return null;
        }


        public async Task<string?> Logout(LogoutUserRequest logoutUserRequest, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var httpRsp = await _httpClient.PostAsJsonAsync(_requestUri + "logout", logoutUserRequest);
            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadAsStringAsync();
            }
            return null;
        }



    }
}


//httpRsp.RequestMessage
//httpRsp.Content.Headers

//var custome‌​rJsonString = await httpRsp.Content.ReadAsStringAsync()
//JsonConvert.DeserializeObject<Customer>(custome‌​rJsonString)

//_httpClient.PostAsync()
//_httpClient.GetAsync()