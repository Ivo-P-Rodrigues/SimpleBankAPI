using SimpleBank.AcctManage.Core.Application.Contracts.Business;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Core.Application.Business
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
        public IEnumerable<Account> GetAllUserAccounts(Guid userId)
        {
            var userAccounts = _unitOfWork.Accounts.GetAllWhere(account => account.UserId == userId);
            return userAccounts;
        }



        //get account of user with movims
        public async Task<(bool, Account?, IEnumerable<Movement>?)> GetAccountWithMovements(Guid accountId, Guid userId)
        {
            Account? account = await _unitOfWork.Accounts.GetAsync(accountId);
            if (account == null) { return (false, null, null); }
            if (account.UserId != userId) { return (true, null, null); }

            var movements = _unitOfWork.Movements.GetAllWhere(movement => movement.AccountId == accountId);

            return (false, account, movements);
        }


        //create account
        public async Task<Account?> CreateAccount(Account account)
        {
            var savedAccount = await _unitOfWork.Accounts.DirectAddAsync(account);
            if (savedAccount == null) { return null; }
            return savedAccount;
        }





    }
}
