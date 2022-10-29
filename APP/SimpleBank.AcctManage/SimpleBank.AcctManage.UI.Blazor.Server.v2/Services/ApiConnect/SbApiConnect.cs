namespace SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect
{
    public class SbApiConnect
    {
        private readonly HttpClient _httpClient;
        private readonly HttpRequestMessageBuilder _httpRequestBuilder;
        private readonly SbLocalStorage _localStorage;
        private readonly IConfiguration _configuration;

        public SbApiConnect(
            HttpClient httpClient,
            HttpRequestMessageBuilder httpRequestBuilder,
            SbLocalStorage sbLocalStorage,
            IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpRequestBuilder = httpRequestBuilder ?? throw new ArgumentNullException(nameof(httpRequestBuilder));
            _localStorage = sbLocalStorage ?? throw new ArgumentNullException(nameof(sbLocalStorage));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        //API CONNECT
        //returns response
        public async Task<HttpResponseMessage?> GetAsync(string requestUri, bool auth = false)
        {
            if (auth)
            {
                var accessToken = await _localStorage.GetAsync("AccessToken");
                if (accessToken == null) { return null; }
                _httpRequestBuilder.AddHeaderAuth(accessToken);
            }
            var request = _httpRequestBuilder.Build(HttpMethod.Get, requestUri);
            return await _httpClient.SendAsync(request); //.Wait();
        }
        public async Task<HttpResponseMessage?> PostAsync(string requestUri, object objToPost, bool auth = false)
        {
            if (objToPost == null) { throw new ArgumentNullException(nameof(objToPost)); }

            if (auth)
            {
                var accessToken = await _localStorage.GetAsync("AccessToken");
                if (accessToken == null) { return null; }
                _httpRequestBuilder.AddHeaderAuth(accessToken);
            }

            var request = _httpRequestBuilder.AddBodyJson(objToPost).Build(HttpMethod.Post, requestUri);
            return await _httpClient.SendAsync(request); //.Wait();
        }
        public async Task<HttpResponseMessage?> PostStoredAsync<TStored>(string requestUri, bool auth = false)
        {
            if (auth)
            {
                var accessToken = await _localStorage.GetAsync("AccessToken");
                if (accessToken == null) { return null; }
                _httpRequestBuilder.AddHeaderAuth(accessToken);
            }

            var localObj = await _localStorage.GetObjAsync(typeof(TStored));
            if (localObj == null) { return null; }

            var request = _httpRequestBuilder.AddBodyJson(localObj).Build(HttpMethod.Post, requestUri);
            return await _httpClient.SendAsync(request); //.Wait();
        }


        //returns from response
        public async Task<object?> GetAndReturnAsync<TObject>(string requestUri, bool auth = false)
        {
            var response = await GetAsync(requestUri, auth);
            if (response == null || !response.IsSuccessStatusCode) { return false; }

            var content = await response.Content.ReadFromJsonAsync(typeof(TObject));
            return content;
        }
        public async Task<object?> PostAndReturnAsync<TObject>(string requestUri, object objToPost, bool auth = false)
        {
            var response = await PostAsync(requestUri, objToPost, auth);
            if (response == null || !response.IsSuccessStatusCode) { return null; }

            var content = await response.Content.ReadFromJsonAsync(typeof(TObject));
            return content;
        }


        public async Task<bool> PostAndStoreAsync<ToStore>(string requestUri, object objToPost, bool auth = false)
        {
            var response = await PostAsync(requestUri, objToPost, auth);
            if(response == null || !response.IsSuccessStatusCode) { return false; }

            var content = await response.Content.ReadFromJsonAsync(typeof(ToStore)); 
            if (content == null) { return false; }

            await _localStorage.SetObjAsync(content);
            return true;
        }
        public async Task<bool> PostStoredAndStoreAsync<TStored, ToStore>(string requestUri, bool auth = false)
        {
            var response = await PostStoredAsync<TStored>(requestUri, auth);
            if (response == null || !response.IsSuccessStatusCode) { return false; }

            var content = await response.Content.ReadFromJsonAsync(typeof(ToStore));
            if (content == null) { return false; }

            await _localStorage.SetObjAsync(content);
            return true;
        }
        public async Task<bool> PostAndUnregisterAsync<ToUnregister>(string requestUri, object objToPost, bool auth = false)
        {
            var response = await PostAsync(requestUri, objToPost, auth);
            if (response == null || !response.IsSuccessStatusCode) { return false; }

            await _localStorage.DeleteObjAsync(typeof(ToUnregister));
            return true;
        }
        public async Task<bool> PostStoredAndUnregisterAsync<TStored, ToUnregister>(string requestUri, bool auth = false)
        {
            var response = await PostStoredAsync<TStored>(requestUri, auth);
            if (response == null || !response.IsSuccessStatusCode) { return false; }

            await _localStorage.DeleteObjAsync(typeof(ToUnregister));
            return true;
        }

    }
}




//_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
//httpRsp.RequestMessage
//httpRsp.Content.Headers

//var custome‌​rJsonString = await httpRsp.Content.ReadAsStringAsync()
//JsonConvert.DeserializeObject<Customer>(custome‌​rJsonString)