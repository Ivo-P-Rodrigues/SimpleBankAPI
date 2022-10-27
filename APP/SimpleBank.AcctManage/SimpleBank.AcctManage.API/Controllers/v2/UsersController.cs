using Microsoft.AspNetCore.Mvc;

namespace SimpleBank.AcctManage.API.Controllers.v2
{
    /// <summary> v2 placeholder </summary>
    [ApiController, ApiVersion("2.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        /// <summary> Test. </summary>
        public UsersController() { }

        /// <summary> Test. </summary>
        [HttpPost]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult ReturnOk() => Ok("Yeehh!");
       

    }
}
