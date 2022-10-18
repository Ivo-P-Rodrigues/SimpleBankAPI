using SimpleBank.BlazorServerApp.Data.Requests;
using SimpleBank.BlazorServerApp.Data.Responses;

namespace SimpleBank.BlazorServerApp.Services
{
    public interface ITransferService
    {
        Task<TransferResponse?> MakeTransfer(TransferRequest transferRequest, string accessToken);
    }
}