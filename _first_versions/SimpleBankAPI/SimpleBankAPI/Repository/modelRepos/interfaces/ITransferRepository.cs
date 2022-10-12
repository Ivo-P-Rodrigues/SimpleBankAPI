using SimpleBankAPI.Models;
using System.Linq.Expressions;

namespace SimpleBankAPI.Repository
{
    public interface ITransferRepository : IBaseRepository<Transfer>
    {
        public bool Exists(int id);




    }
}