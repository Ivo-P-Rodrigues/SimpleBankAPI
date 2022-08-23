using Microsoft.AspNetCore.Mvc;
using SimpleBankAPI.Contracts;
using SimpleBankAPI.Business;
using Microsoft.AspNetCore.Authorization;

namespace SimpleBankAPI.Controllers.v1
{
    [ApiController, ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;

        public UsersController(IUserBusiness userBusiness) =>
            _userBusiness = userBusiness ?? throw new ArgumentNullException(nameof(userBusiness));

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="createUserRequest">All new user details.</param>
        /// <returns>The newly created user.</returns>
        [AllowAnonymous]
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(Contracts.AccountResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateUserResponse>> Create(CreateUserRequest createUserRequest)
        {
            var newUser = await _userBusiness.Create(createUserRequest);
            if (newUser == null) { return BadRequest("Error on creating."); }
            return Ok(newUser); //TODO: CreatedAtAction ?
        }



        //public async Task<ActionResult<LoginUserResponse>> Login(LoginUserRequest loginUserRequest)
        /// <summary>
        /// Login to be granted access to the API.
        /// </summary>
        /// <param name="loginUserRequest">User's param to Login</param>
        /// <returns>A Token</returns>
        [AllowAnonymous]
        [HttpPost("Login"), MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> Login(LoginUserRequest loginUserRequest)
        {
            string? token = await _userBusiness.ProcessLogin(loginUserRequest);
            if (string.IsNullOrEmpty(token)) { return Unauthorized("User credentials are incorrect."); }
            return Ok(token);
        }

        /// <summary>
        /// Logout.
        /// </summary>
        /// <param name="logoutUserRequest">Request to logout.</param>
        /// <returns>A response.</returns>
        [HttpPost("Logout"), MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginUserResponse>> Logout(LogoutUserRequest logoutUserRequest)
        {
            return Ok();
        }


    }
}
