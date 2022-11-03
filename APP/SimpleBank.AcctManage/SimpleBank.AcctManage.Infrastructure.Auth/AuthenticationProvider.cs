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
        private readonly IUserBusiness _userBusiness;

        public AuthenthicationProvider(
            IConfiguration configuration,
            IUserBusiness userBusiness,
            IUserTokenBusiness userTokenBusiness)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration)); 
            _userTokenBusiness = userTokenBusiness ?? throw new ArgumentNullException(nameof(userTokenBusiness));
            _userBusiness = userBusiness ?? throw new ArgumentNullException(nameof(userBusiness));
        }


        public async Task<UserToken?> GetUserTokenAsync(Guid userId)
            => await _userTokenBusiness.GetUserTokenAsync(u => u.UserId == userId);

        public async Task<(UserToken?, string?)> ProcessLoginAsync(Guid userId)
        {
            var userToken = await _userTokenBusiness.GetUserTokenAsync(u => u.UserId == userId);
            var (user, claimsForToken) = await GetUserWithClaims(userId);
            if(user == null) { return (null, null); }

            if (userToken is not null) //if not new user
            {
                //if (userToken!.Active) { return (null, "Already logged in."); }  //don't allow if already logged in
                //else if (userToken.Refresh) { return (null, "Please refresh your connection instead."); } //don't allow if refresh is still possible

                userToken = ReGenerateUserToken(userToken, claimsForToken!);
                userToken = await _userTokenBusiness.DirectUpdateAsync(userToken);
            }
            else //if never logged user
            {
                userToken = GenerateUserToken(user, claimsForToken!);
                userToken = await _userTokenBusiness.DirectAddAsync(userToken);
            }
            return userToken == null ? (null, null) : (userToken, null);
        }

        public async Task<(UserToken?, string?)> ProcessRenewToken(string refreshToken)
        {
            var userToken = await _userTokenBusiness.GetUserTokenAsync(u => u.RefreshToken == refreshToken); 

            if (userToken == null || !userToken.Refresh) { return (null, "Please login instead."); } 
            if (userToken.Active) { return (null, "No need to refresh."); }

            var (user, claimsForToken) = await GetUserWithClaims(userToken.UserId);
            if (user == null) { return (null, null); }

            userToken = ReGenerateUserToken(userToken, claimsForToken!);
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



        #region Inner workings
        private async Task<(User?, IEnumerable<Claim>?)> GetUserWithClaims(Guid userId)
        {
            var user = await _userBusiness.GetUserAsync(userId);
            if (user is null) { return (null, null); }

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Add("sub", ClaimTypes.NameIdentifier);  //for custom claimtype setting

            var claimsForToken = new[]
                {
                    new Claim("sub", user.Username), //sets the  «authState».User.Identity.Name   (by Type: AuthenticationState.ClaimsPrincipal.IIdentity.string ) 
                    new Claim("userId", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("createdAt", user.CreatedAt.ToString()),
                };

            return (user, claimsForToken);
        }

        private UserToken ReGenerateUserToken(UserToken userToken, IEnumerable<Claim> claimsForToken)
        {
            userToken.AccessToken = CreateAccessToken(claimsForToken);
            userToken.AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Authentication:AccessDuration"]));
            userToken.RefreshToken = CreateRefreshToken();
            userToken.RefreshTokenExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Authentication:RefreshDuration"]));

            return userToken;
        }
        private UserToken GenerateUserToken(User user, IEnumerable<Claim> claimsForToken)
        {
            var userToken = new UserToken(
                user.Id,
                accessToken: CreateAccessToken(claimsForToken),
                accessTokenExpiresAt: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Authentication:AccessDuration"])),
                refreshToken: CreateRefreshToken(),
                refreshTokenExpiresAt: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Authentication:RefreshDuration"])));

            return userToken;
        }

        private string CreateAccessToken(IEnumerable<Claim> claimsForToken) =>
            CreateJWTToken(claimsForToken, int.Parse(_configuration["Authentication:AccessDuration"]));
        private string CreateRefreshToken() =>
            CreateJWTToken(null, int.Parse(_configuration["Authentication:RefreshDuration"]));
        private string CreateJWTToken(IEnumerable<Claim>? claimsForToken, int duration)
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
        #endregion



    }
}
