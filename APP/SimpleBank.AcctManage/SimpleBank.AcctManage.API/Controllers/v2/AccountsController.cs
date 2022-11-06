using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SimpleBank.AcctManage.Core.Domain;
using SimpleBank.AcctManage.API.DTModels.v2.Requests;
using SimpleBank.AcctManage.API.DTModels.v2.Responses;
using SimpleBank.AcctManage.API.Mapping.v2;
using SimpleBank.AcctManage.Core.Application.Contracts.Business.v2;

namespace SimpleBank.AcctManage.API.Controllers.v2
{
    /// <summary> Accounts related API actions. </summary>
    [ApiController, ApiVersion("2.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountBusiness _accountBusiness;
        private readonly IEntityMapper _entityMapper;

        /// <summary> Accounts related API actions. </summary>
        public AccountsController(
            IAccountBusiness accountBusiness,
            IEntityMapper entityMapper)
        {
            _accountBusiness = accountBusiness ?? throw new ArgumentNullException(nameof(accountBusiness));
            _entityMapper = entityMapper ?? throw new ArgumentNullException(nameof(entityMapper));
        }



        /// <summary> Get all logged user Accounts. </summary>
        /// <returns>All logged user Accounts</returns>
        /// <response code="200">Ok - Returns all Accounts of logged user.</response>
        /// <response code="204">NoContent - Logged user has no Accounts.</response>
        [HttpGet("GetAll")]
        [Produces("application/json")]
        [MapToApiVersion("2.0")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IEnumerable<ResponseAccount>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ResponseAccount>>> Get()
        {
            await Task.Delay(1); //cheating to make the method async... why? 

            if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value, out Guid userId))
            { return Unauthorized("Invalid access."); }

            var accounts = _accountBusiness.GetAll(userId);
            if (accounts == null || accounts.Count() == 0) { return NoContent(); }

            var userAccountsResponse = _entityMapper.Map(accounts);
            return Ok(userAccountsResponse);
        }


        /// <summary> Get Account of logged user. </summary>
        /// <param name="accountId">Account Id belonging to user.</param>
        /// <returns>Account of logged user.</returns>
        /// <response code="200">Ok - Returns requested Account.</response>
        [HttpGet("{accountId}")]
        [Produces("application/json")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(typeof(ResponseAccount), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseAccount>> Get(Guid accountId)
        {
            await Task.Delay(1); //cheating to make the method async... why? 

            if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value, out Guid userId))
            { return Unauthorized("Invalid access."); }

            var account = _accountBusiness.Get(accountId);
            if (account == null || account.UserId != userId) { return BadRequest("No account to return."); }

            var accountRsp = _entityMapper.Map(account);
            return Ok(accountRsp);
        }




        /// <summary> Create account for logged user. </summary>
        /// <param name="accountRequest">Desired Amount and Currency to create a new Account.</param>
        /// <returns>Created account.</returns>
        /// <response code="201">Returns the newly created Account.</response>
        [HttpPost]
        [Produces("application/json")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create(RequestAccountCreate accountRequest)
        {
            if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value, out Guid userId))
            { return Unauthorized("Invalid access."); }

            var accountToCreate = _entityMapper.Map(accountRequest, userId);

            Account? accountCreated = await _accountBusiness.CreateAsync(accountToCreate);
            if (accountCreated == null) { return BadRequest(); }

            var accountRsp = _entityMapper.Map(accountCreated);

            return CreatedAtAction(nameof(Get), new { accountId = accountRsp.AccountId }, accountRsp);
        }








    }
}
