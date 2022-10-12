using SimpleBankAPI.Core.Models;

namespace SimpleBankAPI.Repository
{
    public interface IMovementRepository : IBaseRepository<Movement>
    {

        Movement? Get(int id);
        Task<Movement?> GetAsync(int id);


    }
}