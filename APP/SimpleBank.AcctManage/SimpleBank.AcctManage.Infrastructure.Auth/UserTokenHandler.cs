using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Core.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleBank.AcctManage.Infrastructure.Auth
{
    public class UserTokenHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public UserTokenHandler(
            IConfiguration configuration,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration)); ;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork)); ;
        }

        private UserToken CreateUserToken(Guid userId)
        {
            var userTokenId = Guid.NewGuid();

            var claimsForToken = new[]
                {
                    //new Claim(ClaimTypes.NameIdentifier, newSessionId.ToString()), 
                    new Claim("sessionId", userTokenId.ToString()),
                    new Claim("userId", userId.ToString())
                };

            var userToken = new UserToken(
                userTokenId,
                userId,
                accessToken:           GenerateAccessToken(claimsForToken),
                accessTokenExpiresAt:  DateTime.Now.AddMinutes(int.Parse(_configuration["Authentication:AccessDuration"])),
                refreshToken:          GenerateRefreshToken(claimsForToken),
                refreshTokenExpiresAt: DateTime.Now.AddMinutes(int.Parse(_configuration["Authentication:RefreshDuration"])));

            return userToken;
        }

        private string GenerateAccessToken(IEnumerable<Claim> claimsForToken) =>
            GenerateJWTToken(claimsForToken, int.Parse(_configuration["Authentication:AccessDuration"]));
        private string GenerateRefreshToken(IEnumerable<Claim> claimsForToken) =>
            GenerateJWTToken(claimsForToken, int.Parse(_configuration["Authentication:RefreshDuration"]));

        private string GenerateJWTToken(IEnumerable<Claim> claimsForToken, int duration)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretKey"]));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.Now,
                DateTime.Now.AddMinutes(duration),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return tokenToReturn;
        }




    }
}