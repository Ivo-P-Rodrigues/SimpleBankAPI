using SimpleBankAPI.Core.Models;

namespace SimpleBankAPI.Repository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User? Get(int id);
        Task<User?> GetAsync(int id);

    }
}