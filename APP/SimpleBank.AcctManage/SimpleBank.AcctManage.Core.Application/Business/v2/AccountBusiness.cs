using SimpleBank.AcctManage.Core.Application.Contracts.Business.v2;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Core.Application.Business.v2
{
    public class AccountBusiness : IAccountBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        public AccountBusiness(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }


        //ACCOUNTS controller v2
        public IEnumerable<Account> GetAll(Guid userId) =>
            _unitOfWork.Accounts.GetAllWhere(account => account.UserId == userId);
        public Account? Get(Guid accountId) =>
            _unitOfWork.Accounts.GetWhere(account => account.Id == accountId);
        public async Task<Account?> CreateAsync(Account account)
        {
            var savedAccount = await _unitOfWork.Accounts.DirectAddAsync(account);
            if (savedAccount == null) { return null; }
            return savedAccount;
        }


        //MOVEMENTS controller v2
        public async Task<bool> CheckIfUserOwnsAccountAsync(Guid accountId, Guid userId) =>
            (await _unitOfWork.Accounts.GetAsync(accountId))?.UserId == userId;









    }
}
