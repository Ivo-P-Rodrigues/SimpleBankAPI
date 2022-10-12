using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Domain;
using SimpleBank.AcctManage.Persistence.Repositories.Common;

namespace SimpleBank.AcctManage.Persistence.Repositories
{
    public class MovementRepository : CommonRepository<Movement>, IMovementRepository
    {
        public MovementRepository(SimpleBankDbContext context) : base(context)
        {
        }




    }
}
