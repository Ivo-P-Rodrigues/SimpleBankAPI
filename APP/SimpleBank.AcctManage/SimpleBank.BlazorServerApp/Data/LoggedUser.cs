namespace SimpleBank.BlazorServerApp.Data
{
    public class LoggedUser
    {
        public Guid UserId { get; set; }
        public string CreatedAt { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string PasswordChangedAt { get; set; }
        public string Username { get; set; }

        //public string Password { get; set; }

        public Guid UserTokenId { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenExpiresAt { get; set; }
        public string RefreshToken { get; set; }
        public string RefreshTokenExpiresAt { get; set; }




    }
}
