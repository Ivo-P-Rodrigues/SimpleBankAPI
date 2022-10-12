using SimpleBankAPI.Contracts;
using SimpleBankAPI.Models;
using SimpleBankAPI.Repository;
using System.Collections.Generic;

namespace SimpleBankAPI.Business
{
    public class AccountBusiness : IAccountBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public AccountBusiness(
            IUnitOfWork unitOfWork,
            ILogger<AccountBusiness> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }



        //get accounts of user
        public async Task<IEnumerable<Contracts.AccountResponse>> GetAllUserAccountsAsync(int userId) =>
            _unitOfWork.HardEntityMapper.MapAccountListModelToContract(
                await _unitOfWork.Accounts.GetAllAsync(account => account.UserId == userId));
        public IEnumerable<Contracts.AccountResponse> GetAllUserAccounts(int userId) =>
            _unitOfWork.HardEntityMapper.MapAccountListModelToContract(
                _unitOfWork.Accounts.GetAll(account => account.UserId == userId));



        //get account of user with movims
        public async Task<AccountMovims?> GetAccount(int accountId) 
        {
            Models.Account? modelsAccount = await _unitOfWork.Accounts.GetAsync(accountId);
            if (modelsAccount == null) { return null; }

            Contracts.AccountResponse contractsAccount = _unitOfWork.HardEntityMapper.MapAccountModelToContract(modelsAccount);

            ICollection<Movim> movims = _unitOfWork.HardEntityMapper.MapMovementEnumerableToMovim(
                await _unitOfWork.Movements.GetAllAsync(movement => movement.AccountId == accountId));

            return new AccountMovims(contractsAccount, movims);
        }


        //create account
        public async Task<Contracts.AccountResponse?> CreateAccount(CreateAccountRequest accountRequest, int userId)
        {
            Models.Account account = _unitOfWork.HardEntityMapper.MapRequestToAccountModel(accountRequest, userId);

            try
            {
                await _unitOfWork.Accounts.AddAsync(account);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Critical error on user creation.");
                return null;
            }
            
            return _unitOfWork.HardEntityMapper.MapAccountModelToContract(account);
        }

        public bool CheckUserOwnsAccount(int userId, int accountId) =>
            _unitOfWork.Accounts.Get(accountId)?.UserId == userId;




    }
}
