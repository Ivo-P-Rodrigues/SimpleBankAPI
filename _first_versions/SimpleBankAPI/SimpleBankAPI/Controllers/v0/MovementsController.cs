using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleBankAPI.Models;
using SimpleBankAPI.Repository;

namespace SimpleBankAPI.Controllers.v0
{
    [ApiController, ApiVersion("0.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MovementsController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public MovementsController(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));


        [HttpGet, MapToApiVersion("0.0")]
        public async Task<ActionResult<IEnumerable<Movement>>> GetAll()
        {
            return Ok(await _unitOfWork.Movements.GetAllAsync());
        }

        [HttpGet("{id}"), MapToApiVersion("0.0")]
        public async Task<ActionResult<Movement>> Get(int id)
        {
            var movement = await _unitOfWork.Movements.GetAsync(id);
            if (movement == null) { return NotFound("No such Movement exists"); }
            return Ok(movement);
        }


        [HttpPost, MapToApiVersion("0.0")]
        public async Task<ActionResult<Movement>> Create(Movement movement)
        {
            if (!ModelState.IsValid) { return BadRequest("Model state not valid."); }

            await _unitOfWork.Movements.AddAsync(movement);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = movement.MovementId }, movement);
        }


        [HttpPut("{id}"), MapToApiVersion("0.0")]
        public async Task<IActionResult> Update(int id, Movement movement)
        {
            if (!_unitOfWork.Movements.Exists(id)) { return NotFound("No such Movement exists to update."); }
            if (id != movement.MovementId) { return BadRequest("Id doesn't match with Movement."); }

            _unitOfWork.Movements.ChangeEntityState(movement);

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
        public async Task<IActionResult> DeleteMovement(int id)
        {
            var movement = await _unitOfWork.Movements.GetAsync(id);
            if (movement == null) { return NotFound("No such Movement exists to delete."); }

            _unitOfWork.Movements.Remove(movement);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

      

    }
}
