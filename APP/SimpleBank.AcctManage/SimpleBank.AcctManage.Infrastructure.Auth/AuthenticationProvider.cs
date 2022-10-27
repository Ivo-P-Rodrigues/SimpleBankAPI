using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SimpleBank.AcctManage.Core.Application.Contracts.Business;
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
        private readonly IUserTokenBusiness _userTokenBusiness;

        public AuthenthicationProvider(
            IConfiguration configuration,
            IUserTokenBusiness userTokenBusiness)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration)); 
            _userTokenBusiness = userTokenBusiness ?? throw new ArgumentNullException(nameof(userTokenBusiness));
        }


        public async Task<UserToken?> GetUserTokenAsync(Guid userId)
            => await _userTokenBusiness.GetUserTokenAsync(u => u.UserId == userId);

        public async Task<(UserToken?, string?)> ProcessLoginAsync(Guid userId)
        {
            var userToken = await _userTokenBusiness.GetUserTokenAsync(u => u.UserId == userId);

            if (userToken is not null) 
            {
                if (userToken!.Active) { return (null, "Already logged in."); }
                else if (userToken.Refresh) { return (null, "Please refresh your connection instead."); }

                userToken = GenerateUserToken(userToken);
                userToken = await _userTokenBusiness.DirectUpdateAsync(userToken);
            }
            else
            {
                userToken = GenerateUserToken(userId);
                userToken = await _userTokenBusiness.DirectAddAsync(userToken);
            }
            return userToken == null ? (null, null) : (userToken, null);
        }

        public async Task<(UserToken?, string?)> ProcessRenewToken(string refreshToken)
        {
            var userToken = await _userTokenBusiness.GetUserTokenAsync(u => u.RefreshToken == refreshToken); 

            if (userToken == null || !userToken.Refresh) { return (null, "Please login instead."); } 
            if (userToken.Active) { return (null, "No need to refresh."); }

            userToken = GenerateUserToken(userToken);
            userToken = await _userTokenBusiness.DirectUpdateAsync(userToken);

            return userToken == null ? (null, null) : (userToken, null);
        }
         
        public async Task<bool?> ProcessLogout(Guid userTokenId)
        {
            var userToken = await _userTokenBusiness.GetUserTokenAsync(x => x.Id == userTokenId);
            if (userToken == null || !userToken.Refresh) { return false; }

            if (userToken.Active) { userToken.AccessTokenExpiresAt = DateTime.UtcNow; }
            userToken.RefreshTokenExpiresAt = DateTime.UtcNow;

            userToken = await _userTokenBusiness.DirectUpdateAsync(userToken);
            return userToken == null ? null : true;
        }


        private UserToken GenerateUserToken(Guid userId)
        {
            var claimsForToken = new[]
                {
                    //new Claim(ClaimTypes.NameIdentifier, newSessionId.ToString()), 
                    new Claim("userId", userId.ToString())
                };

            var userToken = new UserToken(
                userId,
                accessToken: GenerateAccessToken(claimsForToken),
                accessTokenExpiresAt: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Authentication:AccessDuration"])),
                refreshToken: GenerateRefreshToken(claimsForToken),
                refreshTokenExpiresAt: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Authentication:RefreshDuration"])));

            return userToken;
        }
        private UserToken GenerateUserToken(UserToken userToken)
        {
            var claimsForToken = new[]
                {
                    new Claim("userId", userToken.UserId.ToString())
                };

            userToken.AccessToken = GenerateAccessToken(claimsForToken);
            userToken.AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Authentication:AccessDuration"]));
            userToken.RefreshToken = GenerateRefreshToken(claimsForToken);
            userToken.RefreshTokenExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Authentication:RefreshDuration"]));

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
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(duration),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return tokenToReturn;
        }

    }
}
