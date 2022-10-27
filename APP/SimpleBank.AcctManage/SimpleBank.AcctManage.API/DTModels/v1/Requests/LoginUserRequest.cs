namespace SimpleBank.AcctManage.API.DTModels.v1.Requests
{
    /// <summary>
    /// LoginUserRequest - Username and Password to authenthicate.
    /// </summary>
    public class LoginUserRequest
    {
        /// <summary>
        /// Your personal password. 
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Your username.
        /// </summary>
        public string Username { get; set; }

    }
}
