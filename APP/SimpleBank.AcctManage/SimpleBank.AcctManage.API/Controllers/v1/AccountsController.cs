using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SimpleBank.AcctManage.Core.Domain;
using SimpleBank.AcctManage.API.Profile;
using SimpleBank.AcctManage.Core.Application.Contracts.Business;
using SimpleBank.AcctManage.API.DTModels.v1.Requests;
using SimpleBank.AcctManage.API.DTModels.v1.Responses;

namespace SimpleBank.AcctManage.API.Controllers.v1
{
    [ApiController, ApiVersion("1.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountBusiness _accountBusiness;
        private readonly IEntityMapper _entityMapper;

        public AccountsController(
            IAccountBusiness accountBusiness,
            IEntityMapper entityMapper)
        {
            _accountBusiness = accountBusiness ?? throw new ArgumentNullException(nameof(accountBusiness));
            _entityMapper = entityMapper ?? throw new ArgumentNullException(nameof(entityMapper));
        }



        /// <summary>
        /// Get all logged user Accounts.
        /// </summary>
        /// <returns>All logged user Accounts</returns>
        /// <response code="200">Ok - Returns all Accounts of logged user.</response>
        /// <response code="204">NoContent - Logged user has no Accounts.</response>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IEnumerable<AccountResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AccountResponse>>> GetAllAccounts()
        {
            await Task.Delay(1); //cheating to make the method async... why? 
            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value!);
            var accounts = _accountBusiness.GetAllUserAccounts(userId);
            if (accounts == null || accounts.Count() == 0) { return NotFound(); }

            var userAccountsResponse = _entityMapper.MapAccountListModelToContract(accounts);
            return Ok(userAccountsResponse);
        }


        /// <summary>
        /// Get Account with all its Movims of logged user.
        /// </summary>
        /// <param name="accountId">Account Id belonging to user.</param>
        /// <returns>Account with all its Movims of logged user.</returns>
        /// <response code="200">Ok - Returns requested Account.</response>
        [HttpGet("{accountId}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(AccountMovims), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountMovims>> GetAccount(Guid accountId)
        {
            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value!);
            (bool, Account?, IEnumerable<Movement>?) result = await _accountBusiness.GetAccountWithMovements(accountId, userId);

            if (result.Item1) { return Unauthorized("Invalid account access."); } //user doesn't own account
            if (result.Item2 == null) { return NotFound("No account to return."); }

            AccountResponse accountRsp = _entityMapper.MapAccountModelToContract(result.Item2);
            ICollection<Movim>? movims = _entityMapper.MapMovementEnumerableToMovim(result.Item3);

            return Ok(new AccountMovims(accountRsp, movims));
        }



        /// <summary>
        /// Create account for logged user.
        /// </summary>
        /// <param name="accountRequest">Desired Amount and Currency to create a new Account.</param>
        /// <returns>Created account.</returns>
        /// <response code="201">Returns the newly created Account.</response>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountResponse>> CreateAccount(CreateAccountRequest accountRequest)
        {
            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value!);
            var accountToCreate = _entityMapper.MapRequestToAccountModel(accountRequest, userId);

            Account? accountCreated = await _accountBusiness.CreateAccount(accountToCreate);
            if (accountCreated == null) { return BadRequest(); }

            var accountResponse = _entityMapper.MapAccountModelToContract(accountCreated);
            return CreatedAtAction("GetAccount", new { accountId = accountResponse.AccountId }, accountResponse);
        }







    }
}
