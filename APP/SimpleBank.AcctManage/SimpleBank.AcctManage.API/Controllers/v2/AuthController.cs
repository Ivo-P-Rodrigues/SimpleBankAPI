using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SimpleBank.AcctManage.Core.Application.Contracts.Providers;
using SimpleBank.AcctManage.API.DTModels.v2.Requests;
using SimpleBank.AcctManage.API.DTModels.v2.Responses;
using SimpleBank.AcctManage.API.Mapping.v2;
using SimpleBank.AcctManage.Core.Application.Contracts.Business.v2;

namespace SimpleBank.AcctManage.API.Controllers.v2
{
    /// <summary> Auth related API actions. </summary>
    [ApiController, ApiVersion("2.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenthicationProvider _authenthicationProvider;
        private readonly IUserBusiness _userBusiness;
        private readonly IEntityMapper _entityMapper;

        /// <summary> Auth related API actions. </summary>
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
        /// <param name="loginRequest">User's param to Login</param>
        /// <returns>A Token</returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        [Produces("application/json")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(typeof(ResponseAuthLogin), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseAuthLogin>> Login(RequestAuthLogin loginRequest)
        {
            if (!_userBusiness.VerifyUserCredentials(loginRequest.Password, loginRequest.Username, out Guid userId))
            { return Unauthorized("User credentials are incorrect."); }

            var (userToken, possibleError) = await _authenthicationProvider.ProcessLoginAsync(userId);

            if (userToken == null)
            {
                return possibleError == null ?
                    StatusCode(StatusCodes.Status500InternalServerError, "Error on login, please contact our customer support.") :
                    StatusCode(StatusCodes.Status400BadRequest, possibleError);
            }

            var loginResponse = _entityMapper.Map(userToken);
            return Ok(loginResponse);
        }





        /// <summary>Renews the refresh token.</summary>
        /// <returns>Refreshed token.</returns>
        [AllowAnonymous]
        [HttpPost("Renew", Name = "Renew")]
        [Produces("application/json")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(typeof(ResponseAuthLogin), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseAuthLogin>> RenewToken(RequestAuthRefresh renewRequest)
        {
            var (userToken, possibleError) = await _authenthicationProvider.ProcessRenewToken(renewRequest.RefreshToken);

            if (userToken == null)
            {
                return possibleError == null ?
                    StatusCode(StatusCodes.Status500InternalServerError, "Error on renew, please contact our customer support.") :
                    StatusCode(StatusCodes.Status400BadRequest, possibleError);
            }

            var loginResponse = _entityMapper.Map(userToken);
            return Ok(loginResponse);
        }


        /// <summary>Logout. </summary>
        /// <param name="logoutUserRequest">Request to logout.</param>
        /// <returns>A response.</returns>
        [HttpPost("Logout")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Logout(RequestAuthLogout logoutUserRequest)
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



    }
}
