using System.ComponentModel.DataAnnotations;

namespace SimpleBankAPI.Contracts
{
    /// <summary>
    /// LoginUserRequest - Username and Password to authenthicate.
    /// </summary>
    public class LoginUserRequest
    {
        /// <summary>
        /// Your personal password. 
        /// </summary>
        [Required, MinLength(8)]
        public string Password { get; set; }
        /// <summary>
        /// Your username.
        /// </summary>
        [Required, MinLength(8)]
        public string Username { get; set; }   


    }
}
