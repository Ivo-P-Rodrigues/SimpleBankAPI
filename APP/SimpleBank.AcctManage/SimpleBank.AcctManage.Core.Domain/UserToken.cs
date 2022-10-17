using SimpleBank.AcctManage.Core.Domain.Common;

namespace SimpleBank.AcctManage.Core.Domain
{
    public class UserToken : BaseEntity
    {
        public UserToken(Guid userId, string accessToken, DateTime accessTokenExpiresAt, string refreshToken, DateTime refreshTokenExpiresAt)
        {
            UserId = userId;
            AccessToken = accessToken;
            AccessTokenExpiresAt = accessTokenExpiresAt;
            RefreshToken = refreshToken;
            RefreshTokenExpiresAt = refreshTokenExpiresAt;
        }

        public Guid UserId { get; set; }

        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiresAt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }

        public bool Active => AccessTokenExpiresAt > DateTime.Now;
        public bool Refresh => RefreshTokenExpiresAt > DateTime.Now;


    }
}
