using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Business.v1
{
    public interface IAccountBusiness
    {
        IEnumerable<Account> GetAll(Guid userId);
        Task<Account?> CreateAsync(Account account);

        Task<bool> CheckIfUserOwnsAccountAsync(Guid accountId, Guid userId);






        Task<(bool, Account?, IEnumerable<Movement>?)> GetAccountWithMovementsAsync(Guid accountId, Guid userId);
        Task<(bool, Account?, IEnumerable<Movement>?, IEnumerable<AccountDoc>?)> GetAccountWithMovementsAndDocsAsync(Guid accountId, Guid userId);
        Task<bool> SaveAccountDocumentAsync(AccountDoc accountDoc);
        IEnumerable<AccountDoc> GetAccountDocuments(Guid accountId);
        AccountDoc? GetDocument(Guid docId);


    }
}