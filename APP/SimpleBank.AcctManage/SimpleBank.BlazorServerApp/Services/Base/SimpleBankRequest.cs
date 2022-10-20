namespace SimpleBank.BlazorServerApp.Services.Base
{
    public class SimpleBankRequest 
    {
        private readonly HttpRequestMessage _httpRequestMessage;

        public SimpleBankRequest(string method, string requestUri)
        {
            _httpRequestMessage = new HttpRequestMessage(
                method:     new HttpMethod(method),
                requestUri: requestUri
            );

        }



    }
}

