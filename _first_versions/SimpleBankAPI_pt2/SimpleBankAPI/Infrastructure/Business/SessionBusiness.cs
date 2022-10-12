using SimpleBankAPI.Core.Models;
using SimpleBankAPI.Repository;
using System.Security.Claims;
using SimpleBankAPI.Contracts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SimpleBankAPI.Business
{
    public class SessionBusiness : ISessionBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public SessionBusiness(
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            ILogger<UserBusiness> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }



        //MAIN Actions
        public async Task<Session?> ProcessLoginAsync(int userId)
        {
            var session = await CreateAndSaveSessionAsync(userId);
            if (session == null) { return null; }
            return session;
        }
        public async Task<Session?> RefreshSessionAsync(int userId, string refreshToken)
        {
            var userSession = await _unitOfWork.Sessions.GetLastSessionOfUserAsync(userId);

            if (userSession.RefreshToken != refreshToken)
            { return null; }

            //renew session
            userSession.AccessToken = GenerateJWTToken(
                new[]
                {
                    new Claim("sessionId", userSession.SessionId),
                    new Claim("userId", userId.ToString()),
                });
            userSession.RefreshToken = GenerateJWTToken();

            userSession.AccessTokenExpiresAt = DateTime.Now.AddMinutes(int.Parse(_configuration["Authentication:AccessDuration"]));
            userSession.RefreshTokenExpiresAt = DateTime.Now.AddMinutes(int.Parse(_configuration["Authentication:RefreshDuration"]));

            var session = await _unitOfWork.Sessions.DirectUpdateAsync(userSession);
            if (session == null) { return null; }

            return session;
        }
        public async Task<bool> ProcessLogoutAsync(Session session)
        {
            session!.Active = false;
            var sessionUpdated = await _unitOfWork.Sessions.DirectUpdateAsync(session);
            if (sessionUpdated == null)
            { return false; }

            return true;
        }

        public bool VerifyLogoutRequestObj(int userId, LogoutUserRequest logoutUserRequest, out Session session)
        {
            session = _unitOfWork.Sessions.GetLastSessionOfUser(userId);

            if (session == null || logoutUserRequest.SessionId != session.SessionId) { return false; }

            return true;
        }


        //Inner support - Sessions
        private async Task<Session?> CreateAndSaveSessionAsync(int userId) =>
            await _unitOfWork.Sessions.DirectAddAsync(CreateSession(userId));
        private Session CreateSession(int userId)
        {
            var newSessionId = Guid.NewGuid().ToString();

            var claimsForToken = new[]
                {
                    new Claim("sessionId", newSessionId),      // new Claim("sub", newSessionId) || new Claim(ClaimTypes.Sid, session.Id.ToString())
                    new Claim("userId", userId.ToString()),    // new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                };

            var accessToken = GenerateJWTToken(claimsForToken);
            var refreshToken = GenerateJWTToken();

            var accessTokenDuration = DateTime.Now.AddMinutes(int.Parse(_configuration["Authentication:AccessDuration"]));
            var refreshTokenDuration = DateTime.Now.AddMinutes(int.Parse(_configuration["Authentication:RefreshDuration"]));

            var session = new Session(
                newSessionId, userId,
                accessToken, accessTokenDuration,
                refreshToken, refreshTokenDuration);

            return session;
        }


        private string GenerateJWTToken(IEnumerable<Claim>? claimsForToken = null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretKey"]));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var duration = claimsForToken != null ?
                int.Parse(_configuration["Authentication:AccessDuration"]) :
                int.Parse(_configuration["Authentication:RefreshDuration"]);

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.Now,                       // start of Token validity
                DateTime.Now.AddMinutes(duration),  // expiration date
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return tokenToReturn;
        }

    }
}
