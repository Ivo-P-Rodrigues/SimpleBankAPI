using SimpleBankAPI.Core.Models;

namespace SimpleBankAPI.Repository
{
    public interface ITransferRepository : IBaseRepository<Transfer>
    {
        Transfer? Get(int id);
        Task<Transfer?> GetAsync(int id);

    }
}