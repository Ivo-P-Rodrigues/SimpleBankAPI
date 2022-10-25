using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Core.Application.Contracts.Providers;
using SimpleBank.AcctManage.Core.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleBank.AcctManage.Infrastructure.Auth
{
    public class AuthenthicationProvider : IAuthenthicationProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenthicationProvider(
            IConfiguration configuration,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration)); ;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork)); ;
        }


        public async Task<UserToken?> GetUserTokenAsync(Guid userId)
            => await _unitOfWork.UserTokens.GetUserTokenAsync(userId);

        public async Task<(UserToken?, string?)> ProcessLoginAsync(Guid userId)
        {
            var userToken = _unitOfWork.UserTokens.FirstOrDefault(x => x.UserId == userId);

            if (userToken != null) //if not first time user
            {
                if (userToken.Active) { return (null, "Already logged in."); }
                else if (userToken.Refresh) { return (null, "Please refresh your connection instead."); }
            }

            var newUserToken = CreateUserToken(userId);

            if (userToken == null)  //if new user, add token
            {
                newUserToken = await _unitOfWork.UserTokens.DirectAddAsync(newUserToken);
            }
            else  //not new user, update token
            { 
                _unitOfWork.UserTokens.UntrackEntity(userToken);
                newUserToken.Id = userToken.Id;
                newUserToken = await _unitOfWork.UserTokens.DirectUpdateAsync(newUserToken);
            }
            if (newUserToken == null) { return (null, null); }

            return (newUserToken, null);
        }

        public async Task<(UserToken?, string?)> ProcessRenewToken(string refreshToken)
        {
            var userToken = _unitOfWork.UserTokens.FirstOrDefault(x => x.RefreshToken == refreshToken); //must be bad for performance, this could be improved...

            if (userToken == null || !userToken.Refresh) { return (null, "Please login instead."); } //never logged in OR refresh no longer possible
            if (userToken.Active) { return (null, "No need to refresh."); }

            var newUserToken = CreateUserToken(userToken.UserId);
            newUserToken = await _unitOfWork.UserTokens.DirectUpdateAsync(newUserToken);
            if (newUserToken == null) { return (null, null); }

            return (newUserToken, null);
        }

        public async Task<bool?> ProcessLogout(Guid userTokenId)
        {
            var userToken = _unitOfWork.UserTokens.FirstOrDefault(x => x.Id == userTokenId);
            if (userToken == null || !userToken.Refresh) { return false; }

            if (userToken.Active) { userToken.AccessTokenExpiresAt = DateTime.Now; }
            userToken.RefreshTokenExpiresAt = DateTime.Now;

            userToken = await _unitOfWork.UserTokens.DirectUpdateAsync(userToken);
            if (userToken == null) { return null; }

            return true;
        }


        private UserToken CreateUserToken(Guid userId)
        {
            var claimsForToken = new[]
                {
                    //new Claim(ClaimTypes.NameIdentifier, newSessionId.ToString()), 
                    new Claim("userId", userId.ToString())
                };

            var userToken = new UserToken(
                userId,
                accessToken: GenerateAccessToken(claimsForToken),
                accessTokenExpiresAt: DateTime.Now.AddMinutes(int.Parse(_configuration["Authentication:AccessDuration"])),
                refreshToken: GenerateRefreshToken(claimsForToken),
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
