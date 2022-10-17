using SimpleBank.AcctManage.Core.Domain.Common;
using System.Text.Json.Serialization;

namespace SimpleBank.AcctManage.Core.Domain
{
    public class UserToken : BaseEntity
    {
        public UserToken(Guid id, Guid userId, string accessToken, DateTime accessTokenExpiresAt, string refreshToken, DateTime refreshTokenExpiresAt)
        {
            Id = id;
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

        [JsonIgnore]
        public bool Active => AccessTokenExpiresAt > DateTime.UtcNow;
        [JsonIgnore]
        public bool Refresh => RefreshTokenExpiresAt > DateTime.UtcNow;


    }
}
