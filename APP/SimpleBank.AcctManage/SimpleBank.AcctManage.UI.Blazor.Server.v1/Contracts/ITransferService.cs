using SimpleBank.AcctManage.UI.Blazor.Server.v1.Data.Requests;
using SimpleBank.AcctManage.UI.Blazor.Server.v1.Data.Responses;

namespace SimpleBank.AcctManage.UI.Blazor.Server.v1.Contracts
{
    public interface ITransferService
    {
        Task<TransferResponse?> MakeTransfer(TransferRequest transferRequest);

        Task<bool> CheckLocallyIfUserIsLoggedAsync();
    }
}