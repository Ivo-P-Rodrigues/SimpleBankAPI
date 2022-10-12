using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleBankAPI.Models;
using SimpleBankAPI.Repository;

namespace SimpleBankAPI.Controllers.v0
{
    [ApiController, ApiVersion("0.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TransfersController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public TransfersController(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));


        [HttpGet, MapToApiVersion("0.0")]
        public async Task<ActionResult<IEnumerable<Transfer>>> GetAll()
        {
            return Ok(await _unitOfWork.Transfers.GetAllAsync());
        }

        [HttpGet("{id}"), MapToApiVersion("0.0")]
        public async Task<ActionResult<Transfer>> Get(int id)
        {
            var transfer = await _unitOfWork.Transfers.GetAsync(id);
            if (transfer == null) { return NotFound("No such Transfer exists"); }
            return Ok(transfer);
        }


        [HttpPost, MapToApiVersion("0.0")]
        public async Task<ActionResult<Transfer>> Create(Transfer transfer)
        {
            if (!ModelState.IsValid) { return BadRequest("Model state not valid."); }

            await _unitOfWork.Transfers.AddAsync(transfer);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = transfer.TransferId }, transfer);
        }


        [HttpPut("{id}"), MapToApiVersion("0.0")]
        public async Task<IActionResult> Update(int id, Transfer transfer)
        {
            if (!_unitOfWork.Transfers.Exists(id)) { return NotFound("No such Transfer exists to update."); }
            if (id != transfer.TransferId) { return BadRequest("Id doesn't match with Transfer."); }

            _unitOfWork.Transfers.ChangeEntityState(transfer);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                //log
                return Conflict(dbUpdateConcurrencyException); //not for production 
                //throw;
            }

            return NoContent();
        }


        [HttpDelete("{id}"), MapToApiVersion("0.0")]
        public async Task<IActionResult> DeleteTransfer(int id)
        {
            var transfer = await _unitOfWork.Transfers.GetAsync(id);
            if (transfer == null) { return NotFound("No such Transfer exists to delete."); }

            _unitOfWork.Transfers.Remove(transfer);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
        


    }
}
