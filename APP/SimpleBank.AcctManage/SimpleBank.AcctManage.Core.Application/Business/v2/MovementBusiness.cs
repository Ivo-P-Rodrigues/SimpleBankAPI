using SimpleBank.AcctManage.Core.Application.Contracts.Business.v2;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Core.Application.Business.v2
{
    public class MovementBusiness : IMovementBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        public MovementBusiness(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IEnumerable<Movement> GetAll(Guid accountId) =>
            _unitOfWork.Movements.GetAllWhere(m => m.AccountId == accountId);




    }
}
