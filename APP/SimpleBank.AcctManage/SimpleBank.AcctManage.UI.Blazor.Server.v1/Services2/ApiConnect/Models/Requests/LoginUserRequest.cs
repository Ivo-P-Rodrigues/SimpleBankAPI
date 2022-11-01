using System.ComponentModel.DataAnnotations;

namespace SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect.Models.Requests
{
    public class LoginUserRequest
    {
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        public string Password { get; set; }

        [StringLength(100, MinimumLength = 8, ErrorMessage = "Username must be between 8 and 100 characters.")]
        public string Username { get; set; }

    }
}
