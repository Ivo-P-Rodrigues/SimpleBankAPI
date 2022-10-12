using SimpleBank.AcctManage.Core.Domain.Common;

namespace SimpleBank.AcctManage.Core.Domain
{
    public class Session : BaseEntity
    {

        public Session(Guid sessionId, Guid userId, string accessToken, DateTime accessTokenExpiresAt, string refreshToken, DateTime refreshTokenExpiresAt)
        {
            Id = sessionId;
            UserId = userId;
            AccessToken = accessToken;
            AccessTokenExpiresAt = accessTokenExpiresAt;
            RefreshToken = refreshToken;
            RefreshTokenExpiresAt = refreshTokenExpiresAt;
        }
        public Session()
        {
        }


        public Guid UserId { get; set; }
        public bool Active { get; set; } = true;

        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiresAt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }

        public virtual User User { get; set; }


    }
}
