namespace SimpleBank.AcctManage.API.DTModels.v1.Requests
{
    /// <summary>
    /// CreateUserRequest - obj. with needed details for a new User.
    /// </summary>
    public class CreateUserRequest
    {
        /// <summary>
        /// Email adress.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User full name.
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        /// User new password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Choose a Username.
        /// </summary>
        public string Username { get; set; }

    }
}
