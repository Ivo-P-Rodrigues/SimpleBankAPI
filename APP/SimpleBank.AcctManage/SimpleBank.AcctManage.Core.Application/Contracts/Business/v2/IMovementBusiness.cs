using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Business.v2
{
    public interface IMovementBusiness
    {
        IEnumerable<Movement> GetAll(Guid accountId);
    }
}