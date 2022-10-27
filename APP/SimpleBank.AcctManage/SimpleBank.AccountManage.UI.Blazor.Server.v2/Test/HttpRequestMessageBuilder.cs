using System.Net.Http.Headers;

namespace SimpleBank.AccountManage.UI.Blazor.Server.v2.Test
{
    public class HttpRequestMessageBuilder
    {
        private HttpRequestMessage _request;


        public HttpRequestMessageBuilder(HttpMethod method, string requestUri)
        {
            _request = new HttpRequestMessage(method, requestUri);
        }

        public HttpRequestMessage Build() => _request;


        public HttpRequestMessage AddBodyString(string bodyString)
        {
            _request.Content = new StringContent(bodyString);
            return _request;
        }

        public HttpRequestMessage AddBodyJson<T>(string bodyJsonString)
        {
            //JsonSerializerOptions? options;

            _request.Content = JsonContent.Create(bodyJsonString, typeof(T));
            return _request;
        }


        public HttpRequestMessage AddHeaderAuth(string accessToken)
        {
            _request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return _request;
        }

        public HttpRequestMessage AddQuery(string query)
        {
            _request.RequestUri = new Uri(_request.RequestUri!.ToString() + "?" + query); //improve this using proper Uri ctor n props
            return _request;
        }



    }

}