using SimpleBank.AcctManage.UI.Blazor.Server.Data;

namespace SimpleBank.AcctManage.UI.Blazor.Server.Contracts.Services
{
    public interface ITransferService
    {
        Task<bool> Transfer(TransferCreate transferCreate);
    }
}