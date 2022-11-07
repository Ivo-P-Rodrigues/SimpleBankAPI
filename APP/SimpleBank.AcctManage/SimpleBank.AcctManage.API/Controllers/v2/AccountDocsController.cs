using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SimpleBank.AcctManage.Core.Domain;
using SimpleBank.AcctManage.API.DTModels.v2.Requests;
using SimpleBank.AcctManage.API.DTModels.v2.Responses;
using SimpleBank.AcctManage.API.Mapping.v2;
using SimpleBank.AcctManage.Core.Application.Business;
using SimpleBank.AcctManage.Core.Application.Contracts.Business.v2;

namespace SimpleBank.AcctManage.API.Controllers.v2
{
    /// <summary> Movements related API actions. </summary>
    [ApiController, ApiVersion("2.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/accounts/{accountId}/")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountDocsController : ControllerBase
    {
        private readonly IAccountBusiness _accountBusiness;
        private readonly IAccountDocBusiness _accountDocBusiness;
        private readonly IEntityMapper _entityMapper;

        private string[] acceptedFileTypes = new string[2] { "application/pdf", "image/png" };
        private int maxFileSizeBytes = 1048576; //1Mb

        /// <summary> Movements related API actions. </summary>
        public AccountDocsController(
            IAccountBusiness accountBusiness,
            IAccountDocBusiness accountDocBusiness,
            IEntityMapper entityMapper)
        {
            _accountBusiness = accountBusiness ?? throw new ArgumentNullException(nameof(accountBusiness));
            _accountDocBusiness = accountDocBusiness ?? throw new ArgumentNullException(nameof(accountDocBusiness));
            _entityMapper = entityMapper ?? throw new ArgumentNullException(nameof(entityMapper));
        }



        /// <summary> Get all Docs of account. Metadata only.</summary>
        /// <param name="accountId">Account Id belonging to user.</param>
        /// <returns>All Docs of account.</returns>
        /// <response code="200">Ok - Returns requested Docs.</response>
        [HttpGet("GetAllDocs")]
        [Produces("application/json")]
        [MapToApiVersion("2.0")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IEnumerable<ResponseAccountDoc>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ResponseAccountDoc>>> GetAll(Guid accountId)
        {
            if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value, out Guid userId) ||
                !await _accountBusiness.CheckIfUserOwnsAccountAsync(accountId, userId))
            { return Unauthorized("Invalid access."); }

            var accDocs = _accountDocBusiness.GetAll(accountId);
            if (accDocs == null || accDocs.Count() == 0)
            { return NoContent(); }

            var accDocsResponse = _entityMapper.Map(accDocs);
            return Ok(accDocsResponse);
        }

        /// <summary> Get a Doc of account. Metadata only.</summary>
        /// <param name="accountId">Account Id belonging to user.</param>
        /// <param name="docId">Doc Id to get</param>
        /// <returns>Doc of account.</returns>
        /// <response code="200">Ok - Returns requested Doc.</response>
        [HttpGet("GetDoc/{docId}")]
        [Produces("application/json")]
        [MapToApiVersion("2.0")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(ResponseAccountDoc), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseAccountDoc>> Get(Guid accountId, Guid docId)
        {
            if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value, out Guid userId) ||
                !await _accountBusiness.CheckIfUserOwnsAccountAsync(accountId, userId))
            { return Unauthorized("Invalid access."); }

            var accDoc = _accountDocBusiness.Get(docId);
            if (accDoc == null)
            { return BadRequest("No such doc exists."); }

            var accDocResponse = _entityMapper.Map(accDoc);
            return Ok(accDocResponse);
        }





        /// <summary> Upload a Doc in jpg or pdf format for the respective account. </summary>
        /// <param name="file">File to upload.</param>
        /// <param name="accountId">Related account.</param>
        /// <returns>Doc in jpg or pdf.</returns>
        [HttpPost("Upload")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Upload(Guid accountId, [FromForm] IFormFile file)
        {
            //validations
            if (!acceptedFileTypes.Contains(file.ContentType))
            { return BadRequest("Incorrect file type. Only PNG or PDF allowed."); }

            if (file.Length >= maxFileSizeBytes)
            { return BadRequest("File is too big. Max allowed: 1Mb."); }

            if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value, out Guid userId) ||
            !await _accountBusiness.CheckIfUserOwnsAccountAsync(accountId, userId))
            { return BadRequest("Invalid account id."); }

            var saved = await _accountDocBusiness.SaveAccountDocumentAsync(accountId, file.Name, file.ContentType, file.OpenReadStream());
            if (!saved) { return BadRequest(); }

            return Ok("File successfully uploaded.");
        }
        

        /// <summary>Download a Doc in jpg or pdf format from the respective account.</summary>
        /// <param name="docId">Id of the doc to download.</param>
        /// <returns>Doc in jpg or pdf.</returns>
        [HttpGet("Download/{docId}")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(typeof(IEnumerable<AccountDoc>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Download(Guid docId)
        {
            var doc = _accountDocBusiness.Get(docId);
            if (doc == null) { return BadRequest("Invalid document id."); }

            if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value, out Guid userId) || 
                !await _accountBusiness.CheckIfUserOwnsAccountAsync(doc.AccountId, userId))
            { return BadRequest("Invalid document id."); }
             
            return new FileContentResult(doc.Content, doc.DocType)
            {
                FileDownloadName = $"SbAccount-{doc.Id}#{doc.AccountId}.{doc.DocType.Split('/').Last()}",
            };
        }



    }
}
