using SimpleBank.AcctManage.Core.Application.Contracts.Business.v1;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Core.Application.Business.v1
{
    public class AccountBusiness : IAccountBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        public AccountBusiness(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }



        //get accounts of user
        public IEnumerable<Account> GetAll(Guid userId) =>
            _unitOfWork.Accounts.GetAllWhere(account => account.UserId == userId);


        //get account of user with movims
        public async Task<(bool, Account?, IEnumerable<Movement>?)> GetAccountWithMovementsAsync(Guid accountId, Guid userId)
        {
            Account? account = await _unitOfWork.Accounts.GetAsync(accountId);
            if (account == null) { return (false, null, null); }
            if (account.UserId != userId) { return (true, null, null); }

            var movements = _unitOfWork.Movements.GetAllWhere(movement => movement.AccountId == accountId);

            return (false, account, movements);
        }
        //get account of user with movims and docs
        public async Task<(bool, Account?, IEnumerable<Movement>?, IEnumerable<AccountDoc>?)> GetAccountWithMovementsAndDocsAsync(Guid accountId, Guid userId)
        {
            Account? account = await _unitOfWork.Accounts.GetAsync(accountId);
            if (account == null) { return (false, null, null, null); }
            if (account.UserId != userId) { return (true, null, null, null); }

            var movements = _unitOfWork.Movements.GetAllWhere(movement => movement.AccountId == accountId);
            var docs = _unitOfWork.AccountDocs.GetAllWhere(accDoc => accDoc.AccountId == accountId);

            return (false, account, movements, docs);
        }



        //create account
        public async Task<Account?> CreateAsync(Account account)
        {
            var savedAccount = await _unitOfWork.Accounts.DirectAddAsync(account);
            if (savedAccount == null) { return null; }
            return savedAccount;
        }


        //upload account doc
        public async Task<bool> CheckIfUserOwnsAccountAsync(Guid accountId, Guid userId) =>
            (await _unitOfWork.Accounts.GetAsync(accountId))?.UserId == userId;

        public async Task<bool> SaveAccountDocumentAsync(AccountDoc accountDoc) =>
            await _unitOfWork.AccountDocs.DirectAddAsync(accountDoc) != null;

        public IEnumerable<AccountDoc> GetAccountDocuments(Guid accountId) =>
            _unitOfWork.AccountDocs.GetAllWhere(doc => doc.AccountId == accountId);

        public AccountDoc? GetDocument(Guid docId) =>
            _unitOfWork.AccountDocs.GetWhere(doc => doc.Id == docId);

    }
}
