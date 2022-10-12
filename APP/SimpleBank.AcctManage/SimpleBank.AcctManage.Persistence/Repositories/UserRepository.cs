using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Domain;
using SimpleBank.AcctManage.Persistence.Repositories.Common;

namespace SimpleBank.AcctManage.Persistence.Repositories
{
    public class UserRepository : CommonRepository<User>, IUserRepository
    {
        public UserRepository(SimpleBankDbContext context) : base(context)
        {
        }




    }
}
