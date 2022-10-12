

namespace SimpleBankAPI.Contracts
{
    public class CreateUserResponse
    {
        public int UserId { get; set; }
        public string CreatedAt { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string PasswordChangedAt { get; set; }
        public string Username { get; set; }   


    }
}
