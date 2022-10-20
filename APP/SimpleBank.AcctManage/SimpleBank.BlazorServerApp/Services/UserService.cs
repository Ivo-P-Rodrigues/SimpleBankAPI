using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SimpleBank.BlazorServerApp.Contracts;
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

        public UserService(HttpClient httpClient, ProtectedLocalStorage protectedLocalStorage)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _protectedLocalStorage = protectedLocalStorage ?? throw new ArgumentNullException(nameof(protectedLocalStorage));
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
                var response = await httpRsp.Content.ReadFromJsonAsync(typeof(LoginUserResponse)) as LoginUserResponse;
                //var purpose = response!.UserId.ToString(); // TODO : set purpose 
                await _protectedLocalStorage.SetAsync("tokenId", response!.UserTokenId);
                await _protectedLocalStorage.SetAsync("accToken", response!.AccessToken);
                await _protectedLocalStorage.SetAsync("accTokenDate", response!.AccessTokenExpiresAt);
                await _protectedLocalStorage.SetAsync("rfhToken", response!.RefreshToken);
                await _protectedLocalStorage.SetAsync("rfhTokenDate", response!.RefreshTokenExpiresAt);
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


        public async Task<string?> Logout()
        {
            var storageAccToken = await _protectedLocalStorage.GetAsync<string>("accToken");
            if (storageAccToken.Success)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", storageAccToken.Value);
            }
            else return null;

            LogoutUserRequest logoutUserRequest;
            var storageTokenId = await _protectedLocalStorage.GetAsync<string>("tokenId");
            if (storageTokenId.Success)
            {
                logoutUserRequest = new LogoutUserRequest() { UserTokenId = Guid.Parse(storageTokenId.Value!) };
            }
            else return null;
           
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