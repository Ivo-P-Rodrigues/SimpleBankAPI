using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Business.v1
{
    public interface IMovementBusiness
    {
        IEnumerable<Movement> GetAll(Guid accountId);
    }
}