using System.Net.Http.Headers;

namespace SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect
{
    public class HttpRequestMessageBuilder
    {
        private HttpRequestMessage _request = new HttpRequestMessage();

        public HttpRequestMessageBuilder() { }

        public HttpRequestMessage Build() => _request;
        public HttpRequestMessage Build(HttpMethod method, string requestUri) =>
            AddMethod(method).AddRequestUri(requestUri).Build();


        //MAIN
        public HttpRequestMessageBuilder AddMethod(HttpMethod method)
        {
            _request.Method = method;
            return this;
        }
        public HttpRequestMessageBuilder AddRequestUri(string requestUri)
        {
            _request.RequestUri = new Uri(requestUri, UriKind.RelativeOrAbsolute);
            return this;
        }

        //BODY
        public HttpRequestMessageBuilder AddBodyString(string bodyString)
        {
            _request.Content = new StringContent(bodyString);
            return this;
        }
        public HttpRequestMessageBuilder AddBodyJson<T>(T obj)
        {//JsonSerializerOptions? options;
            _request.Content = JsonContent.Create(obj);
            return this;
        }

        //HEADER
        public HttpRequestMessageBuilder AddHeaderAuth(string accessToken)
        {
            _request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return this;
        }





        //Testing
        private HttpRequestMessageBuilder AddQuery(string query)
        {
            _request.RequestUri = new Uri(_request.RequestUri!.ToString() + "?" + query); //improve this using proper Uri ctor n props
            return this;
        }
        private HttpRequestMessageBuilder Test(string test)
        {
            //   _request.Content
            return this;
        }


    }

}