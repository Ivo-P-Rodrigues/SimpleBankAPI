using SimpleBankAPI.Models;
using System.Linq.Expressions;

namespace SimpleBankAPI.Repository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public bool Exists(int id);


       

    }
}