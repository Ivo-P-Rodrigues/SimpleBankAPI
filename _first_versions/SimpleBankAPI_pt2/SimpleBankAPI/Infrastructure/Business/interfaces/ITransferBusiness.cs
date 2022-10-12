using SimpleBankAPI.Contracts;
using SimpleBankAPI.Core.Models;

namespace SimpleBankAPI.Business
{
    public interface ITransferBusiness
    {
        Task<(bool, bool, Transfer?)> MakeTransfer(Transfer transfer, int userId);
    }
}