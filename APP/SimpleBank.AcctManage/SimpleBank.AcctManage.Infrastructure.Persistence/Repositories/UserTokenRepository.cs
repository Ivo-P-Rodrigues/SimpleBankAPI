using Microsoft.EntityFrameworkCore;
using SimpleBank.AcctManage.Core.Domain;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Infrastructure.Persistence.Repositories.Common;
using Serilog;

namespace SimpleBank.AcctManage.Infrastructure.Persistence.Repositories
{
    public class UserTokenRepository : CommonRepository<UserToken>, IUserTokenRepository
    {
        public UserTokenRepository(SimpleBankDbContext context, ILogger logger) : base(context, logger)
        {
        }





    }
}
