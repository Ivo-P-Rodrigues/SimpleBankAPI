namespace SimpleBank.BlazorServerApp.Data.Requests
{
    public class CreateUserRequest
    {
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }

    }
}
