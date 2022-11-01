using SimpleBank.AcctManage.UI.Blazor.Server.Services.Requests;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Services
{
    public class UserService
    {
        private readonly IHttpClientFactory _client;
        private readonly string _requestUri = "/api/v1/users/";

        public UserService(
            IHttpClientFactory client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }


        public async Task<bool> Create(CreateUserRequest createUserRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _requestUri);
            request.Content = JsonContent.Create(createUserRequest);

            var client = _client.CreateClient("SbApi");
            var response = await client.SendAsync(request);

            return response.IsSuccessStatusCode;
        }




    }
}
