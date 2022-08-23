using SimpleBankAPI.Contracts;
using SimpleBankAPI.Models;
using SimpleBankAPI.Repository;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;

namespace SimpleBankAPI.Business
{
    public class TransferBusiness : ITransferBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public TransferBusiness(
            IUnitOfWork unitOfWork,
            ILogger<TransferBusiness> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        //make transfer
        public async Task<TransferResponse?> MakeTransfer(TransferRequest transferRequest)
        {
            if (!ValidateTransferRequest(transferRequest)) { return null; }

            var transfer = _unitOfWork.HardEntityMapper.MapRequestToTransfer(transferRequest);
            _unitOfWork.Transfers.ChangeEntityState(transfer, 4);

            CreateTransferMovements(transfer, out Movement movementFrom, out Movement movementTo);
            _unitOfWork.Movements.ChangeEntityState(movementFrom, 4);
            _unitOfWork.Movements.ChangeEntityState(movementTo, 4);

            var accountFrom = await UpdateAccountBalance(transfer.FromAccountId, -transfer.Amount);
            var accountTo = await UpdateAccountBalance(transfer.ToAccountId, transfer.Amount);
            _unitOfWork.Accounts.ChangeEntityState(accountFrom);
            _unitOfWork.Accounts.ChangeEntityState(accountTo);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Critical error on transfer processing.");
                _unitOfWork.Transfers.ChangeEntityState(transfer, 1);
                _unitOfWork.Movements.ChangeEntityState(movementFrom, 1);
                _unitOfWork.Movements.ChangeEntityState(movementTo, 1);
                _unitOfWork.Accounts.ChangeEntityState(accountFrom, 1);
                _unitOfWork.Accounts.ChangeEntityState(accountTo, 1);
                return null;
            }

            return _unitOfWork.HardEntityMapper.MapTransferToResponse(transfer);
        }

        public bool CheckUserOwnsTransferingAccount(TransferRequest transferRequest, int userId)=> 
            _unitOfWork.Accounts.Get(account => account.AccountId == transferRequest.FromAccountId)?.UserId == userId;

        private void CreateTransferMovements(Transfer transfer, out Movement movementFrom, out Movement movementTo)
        {
            movementFrom = new Movement()
            {
                AccountId = transfer.FromAccountId,
                Amount = -transfer.Amount,
                CreatedAt = DateTime.Now
            };
            movementTo = new Movement() {
                AccountId = transfer.ToAccountId,
                Amount = transfer.Amount,
                CreatedAt = DateTime.Now
            };
        }

        private async Task<Models.Account> UpdateAccountBalance(int accountId, decimal amount)
        {
            var account = await _unitOfWork.Accounts.GetAsync(accountId);
            account.Balance += amount;
            return account;
        }


        //Transfer checks
        private bool CheckEqualAccountsId(TransferRequest transferRequest) =>
            transferRequest.ToAccountId == transferRequest.FromAccountId;
        private bool CheckAccountsExistence(TransferRequest transferRequest) =>
            _unitOfWork.Accounts.Exists(account => account.AccountId == transferRequest.FromAccountId)
            && _unitOfWork.Accounts.Exists(account => account.AccountId == transferRequest.FromAccountId);
        private bool CheckAccountsEqualCurrency(TransferRequest transferRequest) =>
            _unitOfWork.Accounts.Get(account => account.AccountId == transferRequest.FromAccountId)?.Currency
            == _unitOfWork.Accounts.Get(account => account.AccountId == transferRequest.FromAccountId)?.Currency;
        private bool CheckAccountFromHasBalanceToTransfer(TransferRequest transferRequest) =>
            _unitOfWork.Accounts.Get(account => account.AccountId == transferRequest.FromAccountId)?.Balance >= transferRequest.Amount;

        private bool ValidateTransferRequest(TransferRequest transferRequest) =>
                !CheckEqualAccountsId(transferRequest) &&
                CheckAccountsExistence(transferRequest) &&
                CheckAccountsEqualCurrency(transferRequest) &&
                CheckAccountFromHasBalanceToTransfer(transferRequest);






    }
}
