using SimpleBank.AcctManage.Domain;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Persistence.Repositories.Common;

namespace SimpleBank.AcctManage.Persistence.Repositories
{
    public class AccountRepository : CommonRepository<Account>, IAccountRepository
    {
        public AccountRepository(SimpleBankDbContext context) : base(context)
        {
        }



    }
}
