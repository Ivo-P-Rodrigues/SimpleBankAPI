using SimpleBank.AcctManage.Core.Application.Models;
using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Business.v2
{
    public interface IMovementBusiness
    {
        IEnumerable<Movement> GetAll(Guid accountId);
        Task<(IEnumerable<Movement>, PaginationMetadata)> GetPartAsync(string? searchQuery, int pageNumber, int pageSize);
    }
}