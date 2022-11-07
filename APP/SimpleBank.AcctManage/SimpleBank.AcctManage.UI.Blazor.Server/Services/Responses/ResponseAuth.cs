namespace SimpleBank.AcctManage.UI.Blazor.Server.Services.Responses
{

    public class ResponseAuthLogin
    {
        public Guid UserTokenId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

    }



}
