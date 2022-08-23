using Microsoft.EntityFrameworkCore;
using SimpleBankAPI.Models;
using System.Linq.Expressions;

namespace SimpleBankAPI.Repository
{
    public interface IMovementRepository : IBaseRepository<Movement>
    {
        public bool Exists(int id);


      

    }
}