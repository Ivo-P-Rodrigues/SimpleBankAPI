using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Business.v2
{
    public interface IAccountDocBusiness
    {
        AccountDoc? Get(Guid docId);
        IEnumerable<AccountDoc> GetAll(Guid accountId);
        Task<bool> SaveAccountDocumentAsync(Guid accountId, string unsafename, string contentType, Stream docStream);

    }
}