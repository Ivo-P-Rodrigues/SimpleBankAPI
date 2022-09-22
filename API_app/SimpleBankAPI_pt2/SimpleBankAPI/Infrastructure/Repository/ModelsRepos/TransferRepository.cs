using SimpleBankAPI.Core.Models;

namespace SimpleBankAPI.Repository
{
    public class TransferRepository : BaseRepository<Transfer>, IBaseRepository<Transfer>, ITransferRepository
    {
        public TransferRepository(SimpleBankAPIContext context, ILogger<TransferRepository> logger) : base(context, logger){ }

        public virtual Transfer? Get(int id) =>
            _context.Set<Transfer>().Find(id);
        public virtual async Task<Transfer?> GetAsync(int id) =>
            await _context.Set<Transfer>().FindAsync(id);

    }
}