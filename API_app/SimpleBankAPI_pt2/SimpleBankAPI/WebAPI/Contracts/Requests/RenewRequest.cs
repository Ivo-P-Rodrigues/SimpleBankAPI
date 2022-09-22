using System.ComponentModel.DataAnnotations;

namespace SimpleBankAPI.Contracts
{
    public class RenewRequest
    {
        [Required]
        public string RefreshToken { get; set; }


    }
}
