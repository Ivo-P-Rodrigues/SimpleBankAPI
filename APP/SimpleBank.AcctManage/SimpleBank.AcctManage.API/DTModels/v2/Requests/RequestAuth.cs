namespace SimpleBank.AcctManage.API.DTModels.v2.Requests
{

    /// <summary> Request obj to login user. </summary>
    public class RequestAuthLogin
    {
        /// <summary> Your personal password. </summary>
        public string Password { get; set; }

        /// <summary> Your username. </summary>
        public string Username { get; set; }
    }



    /// <summary> Request obj to logout user. </summary>
    public class RequestAuthLogout
    {
        /// <summary> Token Id to logout user. </summary>
        public Guid UserTokenId { get; set; }
    }



    /// <summary> Request obj to refresh user access. </summary>
    public class RequestAuthRefresh
    {
        /// <summary> Refresh token string to refresh user access. </summary>
        public string RefreshToken { get; set; }
    }





}
