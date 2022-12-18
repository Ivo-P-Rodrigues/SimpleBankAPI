using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBank.AcctManage.API.Mapping.v2;
using SimpleBank.AcctManage.Core.Application.Contracts.Providers;
using SimpleBank.AcctManage.API.DTModels.v2.Requests;
using SimpleBank.AcctManage.Core.Domain;
using SimpleBank.AcctManage.Core.Application.Contracts.Business.v2;

namespace SimpleBank.AcctManage.API.Controllers.v2
{
    /// <summary> User related API actions. </summary>
    [ApiController, ApiVersion("2.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IEntityMapper _entityMapper;

        public TestController(
            IUserBusiness userBusiness,
            IEntityMapper entityMapper)
        {
            _userBusiness = userBusiness ?? throw new ArgumentNullException(nameof(userBusiness));
            _entityMapper = entityMapper ?? throw new ArgumentNullException(nameof(entityMapper));
        }



  






    }
}

