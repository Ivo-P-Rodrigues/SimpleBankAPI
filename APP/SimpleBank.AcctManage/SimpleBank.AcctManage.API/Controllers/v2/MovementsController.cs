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
    public class MovementsController : ControllerBase
    {
        private readonly IAccountBusiness _accountBusiness;
        private readonly IMovementBusiness _movementBusiness;
        private readonly IEntityMapper _entityMapper;

        /// <summary> Movements related API actions. </summary>
        public MovementsController(
            IAccountBusiness accountBusiness,
            IMovementBusiness movementBusiness,
            IEntityMapper entityMapper)
        {
            _accountBusiness = accountBusiness ?? throw new ArgumentNullException(nameof(accountBusiness));
            _movementBusiness = movementBusiness ?? throw new ArgumentNullException(nameof(movementBusiness));
            _entityMapper = entityMapper ?? throw new ArgumentNullException(nameof(entityMapper));
        }



        /// <summary> Get all Movements of account. </summary>
        /// <param name="accountId">Account Id belonging to user.</param>
        /// <returns>All Movements of account.</returns>
        /// <response code="200">Ok - Returns requested Movements.</response>
        [HttpGet("GetAll")]
        [Produces("application/json")]
        [MapToApiVersion("2.0")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IEnumerable<ResponseMovement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ResponseMovement>>> Get(Guid accountId)
        {
            if (!Guid.TryParse(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value, out Guid userId) ||
                !await _accountBusiness.CheckIfUserOwnsAccountAsync(accountId, userId))
            { return Unauthorized("Invalid access."); }

            var movements = _movementBusiness.GetAll(accountId);
            if(movements == null || movements.Count() == 0)
            { return NoContent(); }    

            var movementsResponse = _entityMapper.Map(movements);
            return Ok(movementsResponse);
        }







    }
}
