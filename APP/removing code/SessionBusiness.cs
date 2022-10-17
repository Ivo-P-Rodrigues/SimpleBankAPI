using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SimpleBank.AcctManage.Core.Application.Contracts.Business;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Core.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleBank.AcctManage.Core.Application.Business
{
    public class SessionBusiness : ISessionBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public SessionBusiness(
            IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        //MAIN Actions
        public async Task<Session?> ProcessLoginAsync(Guid userId)
        {
            var session = await CreateAndSaveSessionAsync(userId);
            if (session == null) { return null; }
            return session;
        }

        public async Task<Session?> RefreshSessionAsync(Guid userId, string refreshToken)
        {
            var userSession = await _unitOfWork.Sessions.GetLastSessionOfUserAsync(userId);

            if (userSession.RefreshToken != refreshToken)
            { return null; }

            //renew session
            userSession.AccessToken = GenerateJWTToken(
                new[]
                {
                    new Claim("sessionId", userSession.Id.ToString()),
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

        public bool VerifyLogoutRequestObj(Guid userId, Guid sessionId, out Session session)
        {
            session = _unitOfWork.Sessions.GetLastSessionOfUser(userId);

            if (session == null || sessionId != session.Id) { return false; }

            return true;
        }


        //Inner support - Sessions
        private async Task<Session?> CreateAndSaveSessionAsync(Guid userId) =>
            await _unitOfWork.Sessions.DirectAddAsync(CreateSession(userId));

        private Session CreateSession(Guid userId)
        {
            var newSessionId = Guid.NewGuid();

            var claimsForToken = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, newSessionId.ToString()), //test
                    new Claim("sessionId", newSessionId.ToString()),
                    new Claim("userId", userId.ToString())  
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
