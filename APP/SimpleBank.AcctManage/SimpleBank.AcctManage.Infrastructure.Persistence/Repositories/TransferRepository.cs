using SimpleBank.AcctManage.Core.Domain;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Infrastructure.Persistence.Repositories.Common;
using Serilog;

namespace SimpleBank.AcctManage.Infrastructure.Persistence.Repositories
{
    public class TransferRepository : CommonRepository<Transfer>, ITransferRepository
    {
        public TransferRepository(SimpleBankDbContext context, ILogger logger) : base(context, logger)
        {
        }




    }
}
