namespace SimpleBank.AcctManage.UI.Blazor.Server.v2.Services.ApiConnect.Models.Responses
{
    public class LoginUserResponse
    {
        public string AccessToken { get; set; }
        public string AccessTokenExpiresAt { get; set; }
        public string RefreshToken { get; set; }
        public string RefreshTokenExpiresAt { get; set; }
        public Guid UserTokenId { get; set; }

        public Guid UserId { get; set; }


    }
}
