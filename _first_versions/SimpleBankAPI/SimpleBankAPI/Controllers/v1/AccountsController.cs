using Microsoft.AspNetCore.Mvc;
using SimpleBankAPI.Contracts;
using SimpleBankAPI.Business;
using Microsoft.AspNetCore.Authorization;

namespace SimpleBankAPI.Controllers.v1
{
    [ApiController, ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize] // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountsController : ControllerBase
    {

        private readonly IAccountBusiness _accountBusiness;
        public AccountsController(IAccountBusiness accountBusiness) : base() =>
            _accountBusiness = accountBusiness ?? throw new ArgumentNullException(nameof(accountBusiness));


        /// <summary>
        /// Get all logged user Accounts.
        /// </summary>
        /// <returns>All logged user Accounts</returns>
        /// <response code="200">Ok - Returns all Accounts of logged user.</response>
        /// <response code="204">NoContent - Logged user has no Accounts.</response>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(IEnumerable<Contracts.AccountResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Contracts.AccountResponse>>> GetAllAccounts()
        {
            if(!GetUserIdInClaim(out int userId)) { return Unauthorized("Invalid access."); }
            var accounts = await _accountBusiness.GetAllUserAccountsAsync(userId);
            return Ok(accounts);
        }


        /// <summary>
        /// Get Account with all its Movims of logged user.
        /// </summary>
        /// <param name="accountId">Account Id belonging to user.</param>
        /// <returns>Account with all its Movims of logged user.</returns>
        /// <response code="200">Ok - Returns requested Account.</response>
        [HttpGet("{accountId}"), MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(AccountMovims), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountMovims>> GetAccount(int accountId)
        {
            if (!GetUserIdInClaim(out int userId)
                || !_accountBusiness.CheckUserOwnsAccount(userId, accountId))
            { return Unauthorized("Invalid access."); }

            AccountMovims? accountMovims = await _accountBusiness.GetAccount(accountId);
            if(accountMovims == null) { return NoContent(); }
            return Ok(accountMovims);
        }


        //[HttpPost(Name = "CreateAccount")] //name is redundant here

        /// <summary>
        /// Create account for logged user.
        /// </summary>
        /// <param name="accountRequest">Desired Amount and Currency to create a new Account.</param>
        /// <returns>Created account.</returns>
        /// <response code="201">Returns the newly created Account.</response>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(Contracts.AccountResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Contracts.AccountResponse>> CreateAccount(CreateAccountRequest accountRequest)
        {
            if (!GetUserIdInClaim(out int userId)) { return Unauthorized("Invalid access."); }

            Contracts.AccountResponse? account = await _accountBusiness.CreateAccount(accountRequest, userId);
            if(account == null) { return BadRequest(); }

            return CreatedAtAction("GetAccount", new { accountId = account.AccountId }, account);
        }
       

        private bool GetUserIdInClaim(out int userId) =>
            int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value, out userId);

            
           



    }
}
