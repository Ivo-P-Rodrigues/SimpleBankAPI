using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleBankAPI.Models;
using SimpleBankAPI.Repository;

namespace SimpleBankAPI.Controllers.v0
{
    [ApiController, ApiVersion("0.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountsController(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));


        [HttpGet, MapToApiVersion("0.0")]
        public async Task<ActionResult<IEnumerable<Models.Account>>> GetAll()
        {
            return Ok(await _unitOfWork.Accounts.GetAllAsync());
        }

        [HttpGet("{id}"), MapToApiVersion("0.0")]
        public async Task<ActionResult<Account>> Get(int id)
        {
            var account = await _unitOfWork.Accounts.GetAsync(id);
            if (account == null) { return NotFound("No such Account exists"); }
            return Ok(account);
        }


        [HttpPost, MapToApiVersion("0.0")]
        public async Task<ActionResult<Account>> Create(Account account)
        {
            if (!ModelState.IsValid) { return BadRequest("Model state not valid."); }

            await _unitOfWork.Accounts.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = account.AccountId }, account);
        }


        [HttpPut("{id}"), MapToApiVersion("0.0")]
        public async Task<IActionResult> Update(int id, Account account)
        {
            if (!_unitOfWork.Accounts.Exists(id)) { return NotFound("No such Account exists to update."); }
            if (id != account.AccountId) { return BadRequest("Id doesn't match with Account."); }

            _unitOfWork.Accounts.ChangeEntityState(account);

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
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _unitOfWork.Accounts.GetAsync(id);
            if (account == null) { return NotFound("No such Account exists to delete."); }

            _unitOfWork.Accounts.Remove(account);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }


        
    }
}
