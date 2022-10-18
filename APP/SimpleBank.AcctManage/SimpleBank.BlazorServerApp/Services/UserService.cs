using SimpleBank.BlazorServerApp.Data.Requests;
using SimpleBank.BlazorServerApp.Data.Responses;
using System.Text.Json;

namespace SimpleBank.BlazorServerApp.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }


        public async Task<CreateUserResponse?> NewUser(CreateUserRequest createUserRequest)
        {
            
            var httpRsp = await _httpClient.PostAsJsonAsync("/api/users/", createUserRequest);
            if(httpRsp.IsSuccessStatusCode)
            {
                var userJsonString = await httpRsp.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<CreateUserResponse>(userJsonString);

            }
            //httpRsp.RequestMessage
            //httpRsp.Content.Headers

            //var custome‌​rJsonString = await httpRsp.Content.ReadAsStringAsync()
            //JsonConvert.DeserializeObject<Customer>(custome‌​rJsonString)

            return null;
        }








    }
}
