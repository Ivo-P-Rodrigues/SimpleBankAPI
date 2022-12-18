using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBank.AcctManage.API.Mapping.v2;
using SimpleBank.AcctManage.Core.Application.Contracts.Providers;
using SimpleBank.AcctManage.API.DTModels.v2.Requests;
using SimpleBank.AcctManage.Core.Domain;
using SimpleBank.AcctManage.Core.Application.Contracts.Business.v2;

namespace SimpleBank.AcctManage.API.Controllers.v2
{
    /// <summary> User related API actions. </summary>
    [ApiController, ApiVersion("2.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IEntityMapper _entityMapper;

        /// <summary>  Users controller. </summary>
        /// <param name="authenthicationProvider">Infrastructure related to auth.</param>
        /// <param name="userBusiness">User related application.</param>
        /// <param name="entityMapper">Profiler.</param>
        /// <exception cref="ArgumentNullException">If <see cref="IAuthenthicationProvider"/>, <see cref="IUserBusiness"/> or <see cref="IEntityMapper"/> are null.</exception>
        public UsersController(
            IUserBusiness userBusiness,
            IEntityMapper entityMapper)
        {
            _userBusiness = userBusiness ?? throw new ArgumentNullException(nameof(userBusiness));
            _entityMapper = entityMapper ?? throw new ArgumentNullException(nameof(entityMapper));
        }


        /// <summary> Create a new user. </summary>
        /// <param name="createUserRequest">All new user details.</param>
        /// <returns>The newly created user.</returns>
        [AllowAnonymous]
        [HttpPost]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create(RequestUserCreate createUserRequest)
        {
            User user = new()
            {
                Username = createUserRequest.Username,
                Email = createUserRequest.Email,
                Fullname = createUserRequest.Fullname
            };

            var newUser = await _userBusiness.CreateUserAsync(user, createUserRequest.Password);
            if (newUser == null) { return BadRequest("Error on creating. Username or email already in use."); }

            return Ok("User successfully created.");
        }










    }
}

