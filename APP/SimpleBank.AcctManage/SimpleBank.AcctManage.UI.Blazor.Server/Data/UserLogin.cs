using System.ComponentModel.DataAnnotations;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Data
{
    public class UserLogin
    {
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        public string Password { get; set; }
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Username must be between 5 and 12 characters.")]
        public string Username { get; set; }

    }
}






