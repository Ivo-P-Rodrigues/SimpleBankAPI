using SimpleBank.AcctManage.Core.Application.Contracts.Business.v2;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Core.Application.Business.v2
{
    public class AccountDocBusiness : IAccountDocBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        public AccountDocBusiness(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public AccountDoc? Get(Guid docId) =>
            _unitOfWork.AccountDocs.GetWhere(doc => doc.Id == docId);

        public IEnumerable<AccountDoc> GetAll(Guid accountId) =>
            _unitOfWork.AccountDocs.GetAllWhere(m => m.AccountId == accountId);

        public async Task<bool> SaveAccountDocumentAsync(AccountDoc accountDoc) =>
            await _unitOfWork.AccountDocs.DirectAddAsync(accountDoc) != null;


    }
}
