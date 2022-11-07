using SimpleBank.AcctManage.Core.Application.Contracts.Business.v2;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Core.Application.Models;
using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Core.Application.Business.v2
{
    public class MovementBusiness : IMovementBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        public MovementBusiness(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public IEnumerable<Movement> GetAll(Guid accountId) =>
            _unitOfWork.Movements.GetAllWhere(m => m.AccountId == accountId);




        public async Task<(IEnumerable<Movement>, PaginationMetadata)> GetPartAsync(string? searchQuery, int pageNumber, int pageSize)
        {
            var collection = _unitOfWork.Movements as IQueryable<Movement>;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(mov =>
                    mov.AccountId.ToString().Contains(searchQuery) ||
                    mov.Id.ToString().Contains(searchQuery));
            }

            var totalItemCount = collection?.Count() ?? 0;
            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

            var collectionToReturn = collection.OrderBy(mov => mov.Id)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList();

            return (collectionToReturn, paginationMetadata);
        }







    }
}
