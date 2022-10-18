using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SimpleBank.AcctManage.API.Profile;
using SimpleBank.AcctManage.Core.Application.Business;
using SimpleBank.AcctManage.API.DTModels.Responses;
using SimpleBank.AcctManage.API.DTModels.Requests;
using SimpleBank.AcctManage.Core.Domain;
using SimpleBank.AcctManage.Core.Application.Contracts.Business;
using SimpleBank.AcctManage.Core.Application.Contracts.Providers;

namespace SimpleBank.AcctManage.API.Controllers
{
    /// <summary>
    /// User related API actions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly IAuthenthicationProvider _authenthicationProvider;
        private readonly IUserBusiness _userBusiness;
        private readonly IEntityMapper _entityMapper;

        public UsersController(
            IAuthenthicationProvider authenthicationProvider,
            IUserBusiness userBusiness,
            IEntityMapper entityMapper)
        {
            _authenthicationProvider = authenthicationProvider ?? throw new ArgumentNullException(nameof(authenthicationProvider));
            _userBusiness = userBusiness ?? throw new ArgumentNullException(nameof(userBusiness));
            _entityMapper = entityMapper ?? throw new ArgumentNullException(nameof(entityMapper));
        }


        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="createUserRequest">All new user details.</param>
        /// <returns>The newly created user.</returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateUserResponse>> Create(CreateUserRequest createUserRequest)
        {
            User user = new() {
                Username = createUserRequest.Username,
                Email = createUserRequest.Email,
                Fullname = createUserRequest.Fullname };

            var newUser = await _userBusiness.CreateUser(user, createUserRequest.Password);
            if (newUser == null) { return BadRequest("Error on creating. Username or email already in use."); }

            var userResponse = _entityMapper.MapUserToResponse(newUser);

            return Created("", userResponse);
        }


        /// <summary>
        /// Login to be granted access to the API.
        /// </summary>
        /// <param name="loginUserRequest">User's param to Login</param>
        /// <returns>A Token</returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginUserResponse>> Login(LoginUserRequest loginUserRequest)
        {
            if (!_userBusiness.VerifyUserCredentials(loginUserRequest.Password, loginUserRequest.Username, out Guid userId))
                { return Unauthorized("User credentials are incorrect."); }

            var (userToken, possibleError) = await _authenthicationProvider.ProcessLoginAsync(userId);

            if(userToken == null)
            {
                return possibleError == null ?
                    StatusCode(StatusCodes.Status500InternalServerError, "Error on login, please contact our customer support.") :
                    StatusCode(StatusCodes.Status400BadRequest, possibleError);
            }

            var loginResponse = _entityMapper.MapUserTokenToLoginResponse(userToken);
            return Ok(loginResponse);
        }


        /// <summary>
        /// Renews the refresh token.
        /// </summary>
        /// <returns>Refreshed token.</returns>
        [AllowAnonymous]
        [HttpPost("Renew", Name = "Renew")]
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


        /// <summary>
        /// Logout.
        /// </summary>
        /// <param name="logoutUserRequest">Request to logout.</param>
        /// <returns>A response.</returns>
        [HttpPost("Logout")]
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



    }
}
