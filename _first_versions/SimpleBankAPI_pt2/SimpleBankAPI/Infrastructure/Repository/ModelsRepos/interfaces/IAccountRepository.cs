using SimpleBankAPI.Core.Models;

namespace SimpleBankAPI.Repository
{
    public interface IAccountRepository : IBaseRepository<Account>
    {

        Account? Get(int id);
        Task<Account?> GetAsync(int id);


    }
}