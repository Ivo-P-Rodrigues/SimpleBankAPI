using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SimpleBank.AcctManage.API.Profile;
using SimpleBank.AcctManage.Core.Domain;
using SimpleBank.AcctManage.Core.Application.Contracts.Business;
using SimpleBank.AcctManage.API.DTModels.v1.Requests;
using SimpleBank.AcctManage.API.DTModels.v1.Responses;

namespace SimpleBank.AcctManage.API.Controllers.v1
{
    [ApiController, ApiVersion("1.0", Deprecated = false)]
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

        /// <summary>
        /// Make a transfer between two accounts.
        /// </summary>
        /// <param name="transferRequest">Transfer Request obj.</param>
        /// <returns>A TranferResponse obj. with transfer details.</returns>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(TransferResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TransferResponse>> MakeTransfer(TransferRequest transferRequest)
        {
            //make transfer
            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value!);
            var transfer = _entityMapper.MapRequestToTransfer(transferRequest);
            (bool userNotOwner, bool notValid, Transfer? transferSaved) = await _transferBusiness.MakeTransfer(transfer, userId);

            //check if went bad
            if (userNotOwner) { return Unauthorized("Unauthorized account access."); }
            if (notValid) { return BadRequest("Transfer not valid."); }
            if (transferSaved == null || userNotOwner && notValid) { return BadRequest("Error on transfer processing."); }

            //respond
            var transferResponse = _entityMapper.MapTransferToResponse(transferSaved);
            return Created("", transferResponse);
        }











    }
}
