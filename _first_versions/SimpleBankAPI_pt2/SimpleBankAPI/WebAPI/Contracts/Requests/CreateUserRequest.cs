using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SimpleBankAPI.Contracts
{
    /// <summary>
    /// CreateUserRequest - obj. with needed details for a new User.
    /// </summary>
    [Index(nameof(CreateUserRequest.Email), IsUnique = true)] // for DB code first, not validation
    [Index(nameof(CreateUserRequest.Username), IsUnique = true)]
    public class CreateUserRequest
    {
        /// <summary>
        /// Email adress.
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; }
        /// <summary>
        /// User full name.
        /// </summary>
        [Required, StringLength(320, MinimumLength = 16)]
        public string Fullname { get; set; }
        /// <summary>
        /// User new password.
        /// </summary>
        [Required, MinLength(8)]
        public string Password { get; set; }
        /// <summary>
        /// Choose a Username.
        /// </summary>

        [Required, MinLength(8)]
        public string Username { get; set; }   


    }
}
