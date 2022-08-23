using System.ComponentModel.DataAnnotations;

namespace SimpleBankAPI.Contracts
{
    public class LogoutUserRequest
    {
        [Required]
        public string SessionId { get; set; }


    }
}
