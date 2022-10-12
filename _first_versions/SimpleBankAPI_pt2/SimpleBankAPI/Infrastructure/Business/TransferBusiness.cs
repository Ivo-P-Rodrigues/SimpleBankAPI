using SimpleBankAPI.Core.Models;
using SimpleBankAPI.Repository;
using System.Transactions;

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
        public async Task<(bool, bool, Transfer?)> MakeTransfer(Transfer transfer, int userId)
        {
            if (transfer.FromAccountId == transfer.ToAccountId) { return (false, true, null); } //not valid - transfer to self 

            var accountFrom = await _unitOfWork.Accounts.GetAsync(transfer.FromAccountId);
            var accountTo   = await _unitOfWork.Accounts.GetAsync(transfer.ToAccountId);

            if (accountFrom == null || accountTo == null)     { return (false, true, null); }   //not valid - non existing account
            if (accountFrom.Balance < transfer.Amount)        { return (false, true, null); }   //not valid - non enough balance
            if (accountFrom.Currency != accountTo.Currency) { return (false, true, null); }   //not valid - not same currency

            if (accountFrom.UserId != userId) { return (true, false, null); }                   //unauthorized - user doesn't own accountFrom

            Transfer transferProcessed;
            try
            {
                transferProcessed = await ProcressTransferAsync(transfer, accountFrom, accountTo);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Critical error on transfer processing.");
                return (true, true, null);
            }

            return (false, false, transferProcessed);
        }
        private async Task<Transfer> ProcressTransferAsync(Transfer transfer, Account accountFrom, Account accountTo)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))  //https://docs.microsoft.com/en-us/ef/core/saving/transactions
            {
                _unitOfWork.Transfers.Add(transfer);

                CreateTransferMovements(transfer, out Movement movementFrom, out Movement movementTo);
                _unitOfWork.Movements.AddRange(new[] { movementFrom, movementTo });

                accountFrom!.Balance -= transfer.Amount;
                accountTo!.Balance += transfer.Amount;
                _unitOfWork.Accounts.UpdateRange(new[] { accountFrom, accountTo });

                await _unitOfWork.SaveChangesAsync();
                scope.Complete();
            }
            return transfer;
        }


        //Movement making
        private void CreateTransferMovements(Transfer transfer, out Movement movementFrom, out Movement movementTo)
        {
            movementFrom = new Movement()
            {
                AccountId = transfer.FromAccountId,
                Amount = -transfer.Amount,
                CreatedAt = DateTime.Now
            };
            movementTo = new Movement()
            {
                AccountId = transfer.ToAccountId,
                Amount = transfer.Amount,
                CreatedAt = DateTime.Now
            };
        }




    }
}

