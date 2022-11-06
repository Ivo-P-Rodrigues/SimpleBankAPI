namespace SimpleBank.AcctManage.API.DTModels.v2.Responses
{
    public class ResponseUserCreate
    {
        public Guid UserTokenId { get; set; }
        public Guid UserId { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenExpiresAt { get; set; }
        public string RefreshToken { get; set; }
        public string RefreshTokenExpiresAt { get; set; }
    }








}
