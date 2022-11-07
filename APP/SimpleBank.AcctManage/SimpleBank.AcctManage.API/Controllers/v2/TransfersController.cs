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
    [ApiController, ApiVersion("2.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransfersController : ControllerBase
    {
        private readonly ITransferBusiness _transferBusiness;
        private readonly IEntityMapper _entityMapper;

        public TransfersController(
            ITransferBusiness transferBusiness,
            IEntityMapper entityMapper)
        {
            _transferBusiness = transferBusiness ?? throw new ArgumentNullException(nameof(transferBusiness));
            _entityMapper = entityMapper ?? throw new ArgumentNullException(nameof(entityMapper));
        }

        /// <summary> Make a transfer between two accounts. </summary>
        /// <param name="transferRequest">Transfer Request obj.</param>
        /// <returns>A TranferResponse obj. with transfer details.</returns>
        [HttpPost]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> MakeTransfer(RequestTransferCreate transferRequest)
        {
            //make transfer
            if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value, out Guid userId))
            { return Unauthorized("Invalid access."); } //corrupted access token

            var transfer = _entityMapper.Map(transferRequest);
            (bool userNotOwner, bool notValid, Transfer? transferSaved) = await _transferBusiness.MakeTransfer(transfer, userId);

            //check if went bad
            if (userNotOwner) { return BadRequest("Unexisting account."); } //account may exist, user doesn't need to know
            if (notValid) { return BadRequest("Transfer not valid."); }
            if (transferSaved == null || userNotOwner && notValid) { return BadRequest("Error on transfer processing."); }

            //respond
            return Ok("Transfer successfully made.");
        }











    }
}
