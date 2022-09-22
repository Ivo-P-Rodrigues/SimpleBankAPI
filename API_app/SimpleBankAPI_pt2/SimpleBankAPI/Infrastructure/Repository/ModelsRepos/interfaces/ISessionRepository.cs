using Microsoft.EntityFrameworkCore;
using SimpleBankAPI.Core.Models;

namespace SimpleBankAPI.Repository
{
    public interface ISessionRepository : IBaseRepository<Session>
    {
        Session? Get(string id);
        Task<Session?> GetAsync(string id);

        Task<Session?> GetLastSessionOfUserAsync(int userId);
        Session? GetLastSessionOfUser(int userId);


    }
}