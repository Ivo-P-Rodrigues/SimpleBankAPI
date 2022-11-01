
using System.Net;

namespace SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect
{
    public class SbApiConnect
    {
        private readonly HttpClient _httpClient;
        private readonly HttpRequestMessageBuilder _httpRequestBuilder;
        private readonly SbLocalStorage _localStorage;
        private readonly RefreshHandler _refreshHandler;

        public SbApiConnect(
            HttpClient httpClient,
            HttpRequestMessageBuilder httpRequestBuilder,
            SbLocalStorage sbLocalStorage,
            RefreshHandler refreshHandler)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpRequestBuilder = httpRequestBuilder ?? throw new ArgumentNullException(nameof(httpRequestBuilder));
            _localStorage = sbLocalStorage ?? throw new ArgumentNullException(nameof(sbLocalStorage));
            _refreshHandler = refreshHandler ?? throw new ArgumentNullException(nameof(refreshHandler));
        }


        public async Task<object?> GetAndReturnAsync<TObject>(string requestUri, bool auth = false)
        {
            var response = await ProcessRequest(GetAsync(requestUri, auth), auth);
            if (response == null || !response.IsSuccessStatusCode) { return false; }

            var content = await response.Content.ReadFromJsonAsync(typeof(TObject));
            return content;
        }
        public async Task<object?> PostAndReturnAsync<TObject>(string requestUri, object objToPost, bool auth = false)
        {
            var response = await ProcessRequest(PostAsync(requestUri, objToPost, auth), auth);
            if (response == null || !response.IsSuccessStatusCode) { return null; }

            var content = await response.Content.ReadFromJsonAsync(typeof(TObject));
            return content;
        }


        public async Task<bool> PostAndStoreAsync<ToStore>(string requestUri, object objToPost, bool auth = false)
        {
            var response = await ProcessRequest(PostAsync(requestUri, objToPost, auth), auth);
            if (response == null || !response.IsSuccessStatusCode) { return false; }

            var content = await response.Content.ReadFromJsonAsync(typeof(ToStore));
            if (content == null) { return false; }

            await _localStorage.SetObjAsync(content);
            return true;
        }
        public async Task<bool> PostStoredAndStoreAsync<TStored, ToStore>(string requestUri, bool auth = false)
        {
            var response = await ProcessRequest(PostStoredAsync<TStored>(requestUri, auth), auth);
            if (response == null || !response.IsSuccessStatusCode) { return false; }

            var content = await response.Content.ReadFromJsonAsync(typeof(ToStore));
            if (content == null) { return false; }

            await _localStorage.SetObjAsync(content);
            return true;
        }
        public async Task<bool> PostAndUnregisterAsync<ToUnregister>(string requestUri, object objToPost, bool auth = false)
        {
            var response = await ProcessRequest(PostAsync(requestUri, objToPost, auth), auth);
            if (response == null || !response.IsSuccessStatusCode) { return false; }

            await _localStorage.DeleteObjAsync(typeof(ToUnregister));
            return true;
        }
        public async Task<bool> PostStoredAndUnregisterAsync<TStored, ToUnregister>(string requestUri, bool auth = false)
        {
            var response = await ProcessRequest(PostStoredAsync<TStored>(requestUri, auth), auth);
            if (response == null || !response.IsSuccessStatusCode) { return false; }

            await _localStorage.DeleteObjAsync(typeof(ToUnregister));
            return true;
        }



        // INNERWORKS
        private async Task<HttpResponseMessage?> GetAsync(string requestUri, bool auth = false)
        {
            if (!await ResolveAuth(auth)) { return null; }

            var request = _httpRequestBuilder.Build(HttpMethod.Get, requestUri);
            return await _httpClient.SendAsync(request); //.Wait();
        }
        private async Task<HttpResponseMessage?> PostAsync(string requestUri, object objToPost, bool auth = false)
        {
            if (objToPost == null) { throw new ArgumentNullException(nameof(objToPost)); }
            if (!await ResolveAuth(auth)) { return null; }

            var request = _httpRequestBuilder.AddBodyJson(objToPost).Build(HttpMethod.Post, requestUri);
            return await _httpClient.SendAsync(request); //.Wait();
        }
        private async Task<HttpResponseMessage?> PostStoredAsync<TStored>(string requestUri, bool auth = false)
        {
            if (!await ResolveAuth(auth)) { return null; }

            var localObj = await _localStorage.GetObjAsync(typeof(TStored));
            if (localObj == null) { return null; }

            var request = _httpRequestBuilder.AddBodyJson(localObj).Build(HttpMethod.Post, requestUri);
            return await _httpClient.SendAsync(request); //.Wait();
        }
        private async Task<bool> ResolveAuth(bool auth)
        {
            if (auth)
            {
                var accessToken = await _localStorage.GetAsync("AccessToken");
                if (accessToken == null) { return false; }
                _httpRequestBuilder.AddHeaderAuth(accessToken);
            }
            return true;
        }

        /// <summary>
        /// Wrap the API call task with this method to refresh if the task failed with Unauthorized. <para />
        /// Only activates on requests with auth. <para />
        /// "Login" is the exception: <para />
        /// - returns BadRequest instead of Unauthorized if refresh needed; <para />
        /// - has no auth -> refresh must be called in the "login page" if login fails. 
        /// </summary>
        /// <param name="request">The task that returns a HttpResponseMessage</param>
        /// <param name="auth">Same bool param as the request Task. Defines if the request to execute needs JWT token.</param>
        /// <returns></returns>
        private async Task<HttpResponseMessage?> ProcessRequest(Task<HttpResponseMessage?> request, bool auth)
        {
            HttpResponseMessage? response = await request;

            if (auth && response?.StatusCode == HttpStatusCode.Unauthorized) 
            {
                var refreshToken = await _localStorage.GetAsync("RefreshToken");
                if (refreshToken == null) { return null; }

                var refreshed = await _refreshHandler.RefreshAsync(refreshToken);
                if (refreshed == null) { return null; }

                await _localStorage.SetObjAsync(refreshed); //keep LoginUserResponse from refresh call

                response = await request;
            }

            return response;
        }




    }
}




//_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
//httpRsp.RequestMessage
//httpRsp.Content.Headers

//var custome‌​rJsonString = await httpRsp.Content.ReadAsStringAsync()
//JsonConvert.DeserializeObject<Customer>(custome‌​rJsonString)