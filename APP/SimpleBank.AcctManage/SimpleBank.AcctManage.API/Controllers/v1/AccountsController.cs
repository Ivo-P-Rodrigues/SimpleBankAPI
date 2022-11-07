using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SimpleBank.AcctManage.Core.Domain;
using SimpleBank.AcctManage.API.DTModels.v1.Requests;
using SimpleBank.AcctManage.API.DTModels.v1.Responses;
using SimpleBank.AcctManage.API.Mapping.v1;
using SimpleBank.AcctManage.Core.Application.Contracts.Business.v1;

namespace SimpleBank.AcctManage.API.Controllers.v1
{
    [ApiController, ApiVersion("1.0", Deprecated = true)]
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



        /// <summary> Get all logged user Accounts. </summary>
        /// <returns>All logged user Accounts</returns>
        /// <response code="200">Ok - Returns all Accounts of logged user.</response>
        /// <response code="204">NoContent - Logged user has no Accounts.</response>
        [HttpGet]
        [Produces("application/json")]
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

            if(!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value, out Guid userId))
            { return BadRequest("Access token invalid.");}

            var accounts = _accountBusiness.GetAll(userId);
            if (accounts == null || accounts.Count() == 0) { return NotFound(); }

            var userAccountsResponse = _entityMapper.MapAccountListModelToContract(accounts);
            return Ok(userAccountsResponse);
        }


        /// <summary>Get Account with all its Movims of logged user. </summary>
        /// <param name="accountId">Account Id belonging to user.</param>
        /// <returns>Account with all its Movims of logged user.</returns>
        /// <response code="200">Ok - Returns requested Account.</response>
        [HttpGet("{accountId}")]
        [Produces("application/json")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(AccountMovims), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountMovims>> GetAccount(Guid accountId)
        {
            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value!);
            (bool, Account?, IEnumerable<Movement>?) result = await _accountBusiness.GetAccountWithMovementsAsync(accountId, userId);

            if (result.Item1) { return Unauthorized("Invalid account access."); } //user doesn't own account
            if (result.Item2 == null) { return NotFound("No account to return."); }

            AccountResponse accountRsp = _entityMapper.MapAccountModelToContract(result.Item2);
            ICollection<Movim>? movims = _entityMapper.MapMovementEnumerableToMovim(result.Item3);

            return Ok(new AccountMovims(accountRsp, movims));
        }


        /// <summary> Get Account with all its Movims and Docs. </summary>
        /// <param name="accountId">Account Id belonging to user.</param>
        /// <returns>Account with all its Movims and Docs of logged user.</returns>
        /// <response code="200">Ok - Returns requested Account.</response>
        [HttpGet("details/{accountId}")]
        [Produces("application/json")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(AccountMovims), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountDetailsResponse>> GetAccountDetails(Guid accountId)
        {
            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value!);
            (bool, Account?, IEnumerable<Movement>?, IEnumerable<AccountDoc>?) result = await _accountBusiness.GetAccountWithMovementsAndDocsAsync(accountId, userId);

            if (result.Item1) { return Unauthorized("Invalid account access."); } //user doesn't own account
            if (result.Item2 == null) { return NotFound("No account to return."); }

            AccountResponse accountRsp = _entityMapper.MapAccountModelToContract(result.Item2);
            ICollection<Movim>? movims = _entityMapper.MapMovementEnumerableToMovim(result.Item3);

            List<AccountDocResponse>? docs = new List<AccountDocResponse>();
            if (result.Item4 != null && result.Item4.Count() != 0)
            {
                result.Item4.ToList().ForEach(d => docs.Add(
                    new AccountDocResponse()
                    {
                        AccountDocId = d.Id,
                        CreatedAt = d.CreatedAt.ToString(),
                        Name = d.Name,
                        DocType = d.DocType
                    }));
            }

            return Ok(new AccountDetailsResponse(accountRsp, movims, docs));
        }


        /// <summary>
        /// Create account for logged user.
        /// </summary>
        /// <param name="accountRequest">Desired Amount and Currency to create a new Account.</param>
        /// <returns>Created account.</returns>
        /// <response code="201">Returns the newly created Account.</response>
        [HttpPost]
        [Produces("application/json")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AccountResponse>> CreateAccount(CreateAccountRequest accountRequest)
        {
            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value!);
            var accountToCreate = _entityMapper.MapRequestToAccountModel(accountRequest, userId);

            Account? accountCreated = await _accountBusiness.CreateAsync(accountToCreate);
            if (accountCreated == null) { return BadRequest(); }

            var accountResponse = _entityMapper.MapAccountModelToContract(accountCreated);
            return CreatedAtAction("GetAccount", new { accountId = accountResponse.AccountId }, accountResponse);
        }


        [HttpPost("SubmitDoc/{accountId}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UploadAccountDoc([FromForm] IFormFile file, [FromRoute] Guid accountId)
        {
            //validations
            if(file.ContentType != "image/png" && file.ContentType != "application/pdf")
            { return BadRequest("Incorrect file type. Only PNG or PDF allowed."); }

            if (file.Length / 1024 >= 1024)
            { return BadRequest("File is too big. Max allowed: 1Mb."); }

            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value!);
            if(!await _accountBusiness.CheckIfUserOwnsAccountAsync(accountId, userId))
            { return BadRequest("Invalid account id."); }

            //map file to doc obj
            byte[] fileValue;
            using (Stream fileStream = file.OpenReadStream())
            {
                using (BinaryReader br = new BinaryReader(fileStream))
                {
                    fileValue = br.ReadBytes((Int32)fileStream.Length);
                }
            }

            AccountDoc accountDoc = new AccountDoc()
            {
                AccountId = accountId,
                Name = file.FileName,
                Content = fileValue,
                DocType = file.ContentType,
            };

            //save and return
            var saved = await _accountBusiness.SaveAccountDocumentAsync(accountDoc);
            if(!saved) { return BadRequest(); }

            return Ok("File successfully uploaded.");
        }


        [HttpGet("GetDocs/{accountId}")]
        [Produces("application/json")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(IEnumerable<AccountDocResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AccountDocResponse>>> GetAccountDocs(Guid accountId)
        {
            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value!);
            if (!await _accountBusiness.CheckIfUserOwnsAccountAsync(accountId, userId))
            { return BadRequest("Invalid account id."); }

            var docs = _accountBusiness.GetAccountDocuments(accountId);
            if(docs is null) { return NoContent(); }

            var docsResponse = _entityMapper.MapAccountDocToResponse(docs);
            return Ok(docsResponse);
        }



        [HttpGet("DownloadDoc/{docId}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(IEnumerable<AccountDoc>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DownloadDoc(Guid docId)
        {
            var doc = _accountBusiness.GetDocument(docId);
            if (doc == null) { return BadRequest("Invalid document id."); }

            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value!);
            if (!await _accountBusiness.CheckIfUserOwnsAccountAsync(doc.AccountId, userId))
            { return BadRequest("Invalid document id."); }

            return new FileContentResult(doc.Content, doc.DocType)
            {
                FileDownloadName = $"SbAccount-{doc.Id}#{doc.AccountId}.{doc.DocType.Split('/').Last()}",
            }; 

        }

        

    }
}
