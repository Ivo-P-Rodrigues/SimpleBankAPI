using SimpleBank.AccountManage.UI.Blazor.Server.v1.Data.Requests;
using SimpleBank.AccountManage.UI.Blazor.Server.v1.Data.Responses;

namespace SimpleBank.AccountManage.UI.Blazor.Server.v1.Contracts
{
    public interface ITransferService
    {
        Task<TransferResponse?> MakeTransfer(TransferRequest transferRequest);

        Task<bool> CheckLocallyIfUserIsLoggedAsync();
    }
}