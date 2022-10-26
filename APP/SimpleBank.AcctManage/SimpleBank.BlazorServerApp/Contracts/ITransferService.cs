using SimpleBank.BlazorServerApp.Data.Requests;
using SimpleBank.BlazorServerApp.Data.Responses;

namespace SimpleBank.BlazorServerApp.Contracts
{
    public interface ITransferService
    {
        Task<TransferResponse?> MakeTransfer(TransferRequest transferRequest);

        Task<bool> CheckLocallyIfUserIsLoggedAsync();
    }
}