using Microsoft.EntityFrameworkCore;
using SimpleBankAPI.Models;
using System.Linq.Expressions;

namespace SimpleBankAPI.Repository
{
    public class MovementRepository : BaseRepository<Movement>, IBaseRepository<Movement>, IMovementRepository
    {
        private readonly SimpleBankAPIContext _context;
        public MovementRepository(SimpleBankAPIContext context) :base (context)=>
            _context = context ?? throw new ArgumentNullException(nameof(context));



        //user state
        public bool Exists(int id) =>
            _context.Set<Movement>().Any(e => e.MovementId == id);






    }
}