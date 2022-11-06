using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Services
{
    public class SimpleBankClient
    {
        private readonly IHttpClientFactory _client;
        private readonly ProtectedLocalStorage _localStorage;
        private readonly ILogger<SimpleBankClient> _logger;
        private readonly IConfiguration _configuration;

        public SimpleBankClient(
            IHttpClientFactory client,
            ProtectedLocalStorage localStorage,
            ILogger<SimpleBankClient> logger,
            IConfiguration configuration)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        //GET
        public async Task<TResponse?> GetAsync<TResponse>(string requestUri, bool auth = false)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var client = _client.CreateClient("SbApi");
            if (!await SetAuthHeaderAsync(client, auth)) { return default(TResponse); }

            var response = await client.SendAsync(request);
            if (response == null || !response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Get failed. Status code returned: {response?.StatusCode}");
                return default(TResponse);
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.NoContent) //success but nothing returned
            {
                _logger.LogInformation($"Get returned NoContent.");
                return default(TResponse);
            }

            var content = await response.Content.ReadFromJsonAsync<TResponse>();
            if (content == null)
            {
                _logger.LogWarning($"Get failed. Error on parsing response to json");
                return default(TResponse);
            }

            return content;
        }
        public async Task<HttpContent?> GetContentAsync(string requestUri, bool auth = false)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var client = _client.CreateClient("SbApi");
            if (!await SetAuthHeaderAsync(client, auth)) { return null; }

            var response = await client.SendAsync(request);
            if (response == null || !response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Get failed. Status code returned: {response?.StatusCode}");
                return null;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NoContent) //success but nothing returned
            {
                _logger.LogInformation($"Get returned NoContent.");
                return null;
            }
            return response.Content;
        }

        //POST
        public async Task<bool> PostAsync(string requestUri, object requestObj, bool auth = false)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Content = JsonContent.Create(requestObj);

            var client = _client.CreateClient("SbApi");
            if (!await SetAuthHeaderAsync(client, auth)) { return false; }

            var response = await client.SendAsync(request);

            if (response == null || !response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Create failed. Status code returned: {response?.StatusCode}");
            }

            return response!.IsSuccessStatusCode;
        }
        public async Task<TResponse?> PostAsync<TResponse>(string requestUri, object requestObj, bool auth = false)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Content = JsonContent.Create(requestObj);

            var client = _client.CreateClient("SbApi");
            if (!await SetAuthHeaderAsync(client, auth)) { return default(TResponse); }

            var response = await client.SendAsync(request);
            if (response == null || !response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"Create failed. Status code returned: {response?.StatusCode}");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NoContent) //success but nothing returned (on a Post???)
            {
                _logger.LogInformation($"Post returned NoContent.");
                return default(TResponse);
            }

            var content = await response!.Content.ReadFromJsonAsync<TResponse>();
            if (content == null)
            {
                _logger.LogWarning($"Create failed. Error on parsing response to json");
            }

            return content;
        }
        public async Task<bool> PostFileAsync(string requestUri, IBrowserFile file, bool auth = false)
        {
            using (var multipartFormContent = new MultipartFormDataContent())
            {
                StreamContent? fileStreamContent = null;
                try
                {
                    fileStreamContent = new StreamContent(file.OpenReadStream(maxAllowedSize: 1048576));
                    fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                }
                catch(Exception ex)
                {
                    return false;
                }
                multipartFormContent.Add(fileStreamContent, name: "file", fileName: file.Name); //unsafe name

                var client = _client.CreateClient("SbApi");
                if (!await SetAuthHeaderAsync(client, auth)) { return false; }
                var response = await client.PostAsync(requestUri, multipartFormContent);

                if (response == null || !response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Create failed. Status code returned: {response?.StatusCode}");
                }
                return response!.IsSuccessStatusCode;
            }
            
        }



        //Innerworkings
        private async Task<bool> SetAuthHeaderAsync(HttpClient client, bool auth)
        {
            if (auth)
            {
                var accessToken = (await _localStorage.GetAsync<string>(_configuration["StorageCustomKeys:accessToken"])).Value;
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    _logger.LogCritical("Creation failed. Access token not found.");
                    return false;
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
            }
            return true;
        }




    }
}
