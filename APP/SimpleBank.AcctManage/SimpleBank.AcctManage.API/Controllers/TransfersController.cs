using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SimpleBank.AcctManage.API.Providers;
using SimpleBank.AcctManage.API.DTModels.Responses;
using SimpleBank.AcctManage.API.DTModels.Requests;
using SimpleBank.AcctManage.Core.Domain;
using SimpleBank.AcctManage.Core.Application.Contracts.Business;

namespace SimpleBank.AcctManage.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransfersController : ControllerBase
    {
        private readonly IAuthenthicationProvider _authenthicationProvider;
        private readonly ITransferBusiness _transferBusiness;
        private readonly IEntityMapper _entityMapper;

        public TransfersController(
            IAuthenthicationProvider authenthicationProvider,
            ITransferBusiness transferBusiness,
            IEntityMapper entityMapper)
        {
            _authenthicationProvider = authenthicationProvider ?? throw new ArgumentNullException(nameof(authenthicationProvider));
            _transferBusiness = transferBusiness ?? throw new ArgumentNullException(nameof(transferBusiness));
            _entityMapper = entityMapper ?? throw new ArgumentNullException(nameof(entityMapper));
        }

        /// <summary>
        /// Make a transfer between two accounts.
        /// </summary>
        /// <param name="transferRequest">Transfer Request obj.</param>
        /// <returns>A TranferResponse obj. with transfer details.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TransferResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TransferResponse>> MakeTransfer(TransferRequest transferRequest)
        {
            //auth
            var checkAuthorization = await _authenthicationProvider.ValidateAuthorizationAsync(User.Claims);
            if (checkAuthorization != null) { return checkAuthorization; }

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
