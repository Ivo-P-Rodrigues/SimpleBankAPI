using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Domain;
using SimpleBank.AcctManage.Persistence.Repositories.Common;

namespace SimpleBank.AcctManage.Persistence.Repositories
{
    public class TransferRepository : CommonRepository<Transfer>, ITransferRepository
    {
        public TransferRepository(SimpleBankDbContext context) : base(context)
        {
        }




    }
}
