using System.ComponentModel.DataAnnotations;

namespace SimpleBank.AccountManage.UI.Blazor.Server.v1.Data.Requests
{
    public class CreateUserRequest
    {
        [StringLength(320, ErrorMessage = "Email address shouldn't be longer than 320 characters.")]
        [EmailAddress(ErrorMessage = "Please provide a correct email address.")]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 16, ErrorMessage = "Fullname must be between 16 and 100 characters.")]
        public string Fullname { get; set; }

        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        public string Password { get; set; }

        [StringLength(100, MinimumLength = 8, ErrorMessage = "Username must be between 8 and 100 characters.")]
        public string Username { get; set; }

    }
}
