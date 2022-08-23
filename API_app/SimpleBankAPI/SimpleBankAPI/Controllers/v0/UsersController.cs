using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleBankAPI.Models;
using SimpleBankAPI.Repository;

namespace SimpleBankAPI.Controllers.v0
{
    [ApiController, ApiVersion("0.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));


        [HttpGet, MapToApiVersion("0.0")]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            return Ok(await _unitOfWork.Users.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await _unitOfWork.Users.GetAsync(id);
            if (user == null) { return NotFound("No such User exists"); }
            return Ok(user);
        }


        [HttpPost, MapToApiVersion("0.0")]
        public async Task<ActionResult<User>> Create(User user)
        {
            if (!ModelState.IsValid) { return BadRequest("Model state not valid."); }

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = user.UserId }, user);
        }


        [HttpPut("{id}"), MapToApiVersion("0.0")]
        public async Task<IActionResult> Update(int id, User user)
        {
            if (!_unitOfWork.Users.Exists(id)) { return NotFound("No such User exists to update."); }
            if (id != user.UserId) { return BadRequest("Id doesn't match with User."); }

            _unitOfWork.Users.ChangeEntityState(user);

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
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _unitOfWork.Users.GetAsync(id);
            if (user == null) { return NotFound("No such User exists to delete."); }

            _unitOfWork.Users.Remove(user);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }



    }
}
