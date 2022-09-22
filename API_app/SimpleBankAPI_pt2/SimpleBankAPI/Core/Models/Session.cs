using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBankAPI.Core.Models
{
    public class Session : Entity
    {
        public Session(string sessionId, int userId, string accessToken, DateTime accessTokenExpiresAt, string refreshToken, DateTime refreshTokenExpiresAt)
        {
            SessionId = sessionId;
            UserId = userId;
            AccessToken = accessToken;
            AccessTokenExpiresAt = accessTokenExpiresAt;
            RefreshToken = refreshToken;
            RefreshTokenExpiresAt = refreshTokenExpiresAt; 
        }

        public string SessionId { get; set; } //= Guid.NewGuid().ToString();
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public bool Active { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiresAt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }

        public virtual User User { get; set; }

    }
}
