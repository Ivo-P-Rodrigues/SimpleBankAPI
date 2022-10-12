using Microsoft.AspNetCore.Mvc;
using SimpleBankAPI.Contracts;
using SimpleBankAPI.Business;
using Microsoft.AspNetCore.Authorization;

namespace SimpleBankAPI.Controllers.v1
{
    [ApiController, ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class TransfersController : ControllerBase
    {
        private readonly ITransferBusiness _transferBusiness;
        public TransfersController(ITransferBusiness transferBusiness) : base() =>
            _transferBusiness = transferBusiness ?? throw new ArgumentNullException(nameof(transferBusiness));


        /// <summary>
        /// Make a transfer between two accounts.
        /// </summary>
        /// <param name="transferRequest">Transfer Request obj.</param>
        /// <returns>A TranferResponse obj. with transfer details.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(TransferResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(Contracts.AccountResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TransferResponse>> MakeTransfer(TransferRequest transferRequest)
        {
            if (!GetUserIdInClaim(out int userId)
                || !_transferBusiness.CheckUserOwnsTransferingAccount(transferRequest, userId))
            { return Unauthorized("Invalid access."); }
            
            TransferResponse? transferResponse = await _transferBusiness.MakeTransfer(transferRequest);
            if (transferResponse == null) { return BadRequest("Invalid transfer request."); }

            return Ok(transferResponse);
         }


        private bool GetUserIdInClaim(out int userId) =>
            int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value, out userId);

    }
}
