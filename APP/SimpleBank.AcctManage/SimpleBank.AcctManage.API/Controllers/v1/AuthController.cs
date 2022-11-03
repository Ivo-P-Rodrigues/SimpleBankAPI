using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SimpleBank.AcctManage.API.Profile;
using SimpleBank.AcctManage.Core.Application.Contracts.Business;
using SimpleBank.AcctManage.Core.Application.Contracts.Providers;
using SimpleBank.AcctManage.API.DTModels.v1.Requests;
using SimpleBank.AcctManage.API.DTModels.v1.Responses;

namespace SimpleBank.AcctManage.API.Controllers.v1
{
    /// <summary> Auth related API actions. </summary>
    [ApiController, ApiVersion("1.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenthicationProvider _authenthicationProvider;
        private readonly IUserBusiness _userBusiness;
        private readonly IEntityMapper _entityMapper;

        public AuthController(
            IAuthenthicationProvider authenthicationProvider,
            IUserBusiness userBusiness,
            IEntityMapper entityMapper)
        {
            _authenthicationProvider = authenthicationProvider ?? throw new ArgumentNullException(nameof(authenthicationProvider));
            _userBusiness = userBusiness ?? throw new ArgumentNullException(nameof(userBusiness));
            _entityMapper = entityMapper ?? throw new ArgumentNullException(nameof(entityMapper));
        }


        /// <summary>Login to be granted access to the API.</summary>
        /// <param name="loginUserRequest">User's param to Login</param>
        /// <returns>A Token</returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginUserResponse>> Login(LoginUserRequest loginUserRequest)
        {
            if (!_userBusiness.VerifyUserCredentials(loginUserRequest.Password, loginUserRequest.Username, out Guid userId))
            { return Unauthorized("User credentials are incorrect."); }

            var (userToken, possibleError) = await _authenthicationProvider.ProcessLoginAsync(userId);

            if (userToken == null)
            {
                return possibleError == null ?
                    StatusCode(StatusCodes.Status500InternalServerError, "Error on login, please contact our customer support.") :
                    StatusCode(StatusCodes.Status400BadRequest, possibleError);
            }

            var loginResponse = _entityMapper.MapUserTokenToLoginResponse(userToken);
            return Ok(loginResponse);
        }


        /// <summary>Renews the refresh token.</summary>
        /// <returns>Refreshed token.</returns>
        [AllowAnonymous]
        [HttpPost("Renew", Name = "Renew")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginUserResponse>> RenewToken(RenewRequest renewRequest)
        {
            var (userToken, possibleError) = await _authenthicationProvider.ProcessRenewToken(renewRequest.RefreshToken);

            if (userToken == null)
            {
                return possibleError == null ?
                    StatusCode(StatusCodes.Status500InternalServerError, "Error on renew, please contact our customer support.") :
                    StatusCode(StatusCodes.Status400BadRequest, possibleError);
            }

            var loginResponse = _entityMapper.MapUserTokenToLoginResponse(userToken);
            return Ok(loginResponse);
        }


        /// <summary>Logout. </summary>
        /// <param name="logoutUserRequest">Request to logout.</param>
        /// <returns>A response.</returns>
        [HttpPost("Logout")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Logout(LogoutUserRequest logoutUserRequest)
        {
            var result = await _authenthicationProvider.ProcessLogout(logoutUserRequest.UserTokenId);

            switch (result)
            {
                case null:
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error on logout, please contact our customer support.");
                case false:
                    return StatusCode(StatusCodes.Status400BadRequest, "Logout unavailable.");
                default:
                    return Ok("User logged out.");

            }

        }


        /// <summary>Gets the user token (as LoginUserResponse). </summary>
        /// <returns>User token.</returns>
        [AllowAnonymous]
        [HttpPost("GetTokenAgain")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginUserResponse>> GetUserToken(LoginUserRequest loginUserRequest)
        {
            if (!_userBusiness.VerifyUserCredentials(loginUserRequest.Password, loginUserRequest.Username, out Guid userId))
            { return Unauthorized("User credentials are incorrect."); }

            var userToken = await _authenthicationProvider.GetUserTokenAsync(userId);
            if (userToken == null) { return BadRequest("Error on getting token."); }

            var loginResponse = _entityMapper.MapUserTokenToLoginResponse(userToken);

            return Ok(loginResponse);
        }

    }
}
