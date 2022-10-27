using Microsoft.AspNetCore.Components;
using SimpleBank.AccountManage.UI.Blazor.Server.v2.Data.Requests;
using SimpleBank.AccountManage.UI.Blazor.Server.v2.Data.Responses;
using System.Net.Http;

namespace SimpleBank.AccountManage.UI.Blazor.Server.v2.Services.ApiConnect
{
    public class SbApiConnect
    {
        private readonly HttpClient _httpClient;
        private readonly HttpRequestMessageBuilder _httpRequestBuilder;
        private readonly SbLocalStorage _sbLocalStorage;
        private readonly IConfiguration _configuration;

        public SbApiConnect(
            HttpClient httpClient,
            HttpRequestMessageBuilder httpRequestBuilder,
            SbLocalStorage sbLocalStorage,
            IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpRequestBuilder = httpRequestBuilder ?? throw new ArgumentNullException(nameof(httpRequestBuilder));
            _sbLocalStorage = sbLocalStorage ?? throw new ArgumentNullException(nameof(sbLocalStorage));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        //LOCAL STORAGE
        public async Task RegisterObj(object obj) =>
            await _sbLocalStorage.SetObjAsync(obj);
        public async Task UnRegisterObj(object obj) =>
            await _sbLocalStorage.DeleteObjAsync(obj);

        public async Task<string?> GetUserTokenId() =>
            await _sbLocalStorage.GetAsync("UserTokenId");


        //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);



        //API CONNECT
        public async Task<HttpResponseMessage?> GetAsync(string requestUri, string accessToken)
        {
            var request = _httpRequestBuilder.AddHeaderAuth(accessToken).Build(HttpMethod.Get, requestUri);
            return await _httpClient.SendAsync(request); //.Wait();
        }
        public async Task<HttpResponseMessage?> GetAsync(string requestUri)
        {
            var request = _httpRequestBuilder.Build(HttpMethod.Get, requestUri);
            return await _httpClient.SendAsync(request); //.Wait();
        }

        public async Task<HttpResponseMessage?> PostAsync<TValue>(string requestUri, TValue value, string accessToken)
        {
            var request = _httpRequestBuilder.AddHeaderAuth(accessToken).AddBodyJson(value).Build(HttpMethod.Post, requestUri);
            return await _httpClient.SendAsync(request); //.Wait();
        }
        public async Task<HttpResponseMessage?> PostAsync<TValue>(string requestUri, TValue value)
        {
            var request = _httpRequestBuilder.AddBodyJson(value).Build(HttpMethod.Post, requestUri);
            return await _httpClient.SendAsync(request); //.Wait();
        }





        public async Task<bool> Refresh()
        {
            var refreshToken = await _sbLocalStorage.GetAsync("RefreshToken");
            if(refreshToken == null) { return false; }

            var renewRequest = new RenewRequest() { RefreshToken = refreshToken };
            var httpRsp = await _httpClient.PostAsJsonAsync(_configuration["SbApiEndPoints:Refresh"], renewRequest);
            if(httpRsp == null || !httpRsp.IsSuccessStatusCode) { return false; }

            var response = await httpRsp.Content.ReadFromJsonAsync(typeof(LoginUserResponse)) as LoginUserResponse;
            if (response == null) { return false; }

            await _sbLocalStorage.SetObjAsync(response!);
            return true;
        }






    }
}
