
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SimpleBank.BlazorServerApp.Data;
using SimpleBank.BlazorServerApp.Data.Responses;
using System.Net.Http.Headers;

namespace SimpleBank.BlazorServerApp.Services.Base
{
    public class SimpleBankClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private ProtectedLocalStorage _localStorage;

        public SimpleBankClient(ProtectedLocalStorage localStorage)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7074/api/") };
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage)); ;
        }


        public Uri BaseAddress
        {
            get =>
                _httpClient.BaseAddress ?? throw new ArgumentNullException(nameof(BaseAddress));
        }



        public void SetAuthHeader(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }




        /*
        public async Task<IEnumerable<AccountResponse>?> GetAllAccounts(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var httpRsp = await _httpClient.PostAsJsonAsync(_requestUri, "");
            if (httpRsp.IsSuccessStatusCode)
            {
                return await httpRsp.Content.ReadFromJsonAsync(typeof(IEnumerable<AccountResponse>)) as IEnumerable<AccountResponse>;
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
         */











        public void Dispose()
        {
            _httpClient.Dispose();
            //GB this
        }


    }
}
