using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SimpleBank.AcctManage.API.Providers;
using SimpleBank.AcctManage.Core.Application.Business;
using SimpleBank.AcctManage.API.DTModels.Responses;
using SimpleBank.AcctManage.API.DTModels.Requests;
using SimpleBank.AcctManage.Core.Domain;
using SimpleBank.AcctManage.Core.Application.Contracts.Business;

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
        private readonly ISessionBusiness _sessionBusiness;
        private readonly IEntityMapper _entityMapper;

        public UsersController(
            IAuthenthicationProvider authenthicationProvider,
            IUserBusiness userBusiness,
            ISessionBusiness sessionBusiness,
            IEntityMapper entityMapper)
        {
            _authenthicationProvider = authenthicationProvider ?? throw new ArgumentNullException(nameof(authenthicationProvider));
            _userBusiness = userBusiness ?? throw new ArgumentNullException(nameof(userBusiness));
            _sessionBusiness = sessionBusiness ?? throw new ArgumentNullException(nameof(sessionBusiness));
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

            var checkAuthorization = await _authenthicationProvider.ValidateAuthorizationOnLoginAsync(userId);
            if (checkAuthorization != null) { return checkAuthorization; }

            var session = await _sessionBusiness.ProcessLoginAsync(userId);
            if (session == null) { return Problem("Error on Login."); }

            var loginResponse = _entityMapper.MapSessionToLoginResponse(session);
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
        public async Task<ActionResult<LoginUserResponse>> RenewRefreshToken(RenewRequest renewRequest)
        {
            var checkAuthorization = await _authenthicationProvider.ValidateAuthorizationOnRefreshAsync(User.Claims);
            if (checkAuthorization != null) { return checkAuthorization; }

            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value!);
            var session = await _sessionBusiness.RefreshSessionAsync(userId, renewRequest.RefreshToken);

            if(session == null) { return BadRequest("Invalid refresh token."); }

            var loginResponse = _entityMapper.MapSessionToLoginResponse(session);
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
            var checkAuthorization = await _authenthicationProvider.ValidateAuthorizationOnLogoutAsync(User.Claims);
            if (checkAuthorization != null) { return checkAuthorization; }

            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value!);
            if(!_sessionBusiness.VerifyLogoutRequestObj(userId, logoutUserRequest.SessionId, out Session session))
                { return BadRequest("Invalid logout"); }

            var logoutCheck = await _sessionBusiness.ProcessLogoutAsync(session);
            if(!logoutCheck) { return BadRequest("No session to logout"); }

            return Ok("Session ended.");
        }



    }
}
