using Microsoft.EntityFrameworkCore;
using SimpleBankAPI.Models;
using System.Linq.Expressions;

namespace SimpleBankAPI.Repository
{
    public class TransferRepository : BaseRepository<Transfer>, IBaseRepository<Transfer>, ITransferRepository
    {
        private readonly SimpleBankAPIContext _context;
        public TransferRepository(SimpleBankAPIContext context) : base(context) =>
            _context = context ?? throw new ArgumentNullException(nameof(context));


        //transfer state
        public bool Exists(int id) =>
            _context.Set<Transfer>().Any(e => e.TransferId == id);







    }
}