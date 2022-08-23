using SimpleBankAPI.Contracts;

namespace SimpleBankAPI.Business
{
    public interface ITransferBusiness
    {
        Task<TransferResponse?> MakeTransfer(TransferRequest transferRequest);

        bool CheckUserOwnsTransferingAccount(TransferRequest transferRequest, int userId);
    }
}