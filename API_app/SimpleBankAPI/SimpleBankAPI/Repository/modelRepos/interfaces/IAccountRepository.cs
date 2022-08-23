using SimpleBankAPI.Models;
using System.Linq.Expressions;

namespace SimpleBankAPI.Repository
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        public bool Exists(int id);




    }
}