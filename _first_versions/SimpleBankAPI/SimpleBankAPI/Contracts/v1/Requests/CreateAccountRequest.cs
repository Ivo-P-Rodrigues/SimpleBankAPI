using System.ComponentModel.DataAnnotations;

namespace SimpleBankAPI.Contracts
{
    /// <summary>
    /// CreateAccountRequest - obj. with needed details for a new Account.
    /// </summary>
    public class CreateAccountRequest
    {
        /// <summary>
        /// The Amount of money the Account starts with. 
        /// </summary>
        [Required]
        public decimal Amount { get; set; } 

        /// <summary>
        /// The Currency of the money in the Account.
        /// </summary>
        [Required, StringLength(3, MinimumLength = 3)] 
        public string Currency { get; set; } = "EUR";



    }
}
