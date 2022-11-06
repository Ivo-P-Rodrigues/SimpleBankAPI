using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Business.v2
{
    public interface ITransferBusiness
    {
        Task<(bool, bool, Transfer?)> MakeTransfer(Transfer transfer, Guid userId);
    }
}