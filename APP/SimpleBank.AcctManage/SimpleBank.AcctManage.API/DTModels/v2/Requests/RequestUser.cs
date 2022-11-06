namespace SimpleBank.AcctManage.API.DTModels.v2.Requests
{

    /// <summary> Request obj to create new user. </summary>
    public class RequestUserCreate
    {
        /// <summary> Email adress. </summary>
        public string Email { get; set; }

        /// <summary> User full name. </summary>
        public string Fullname { get; set; }

        /// <summary> User new password. </summary>
        public string Password { get; set; }

        /// <summary> Choose a Username. </summary>
        public string Username { get; set; }


    }





}
