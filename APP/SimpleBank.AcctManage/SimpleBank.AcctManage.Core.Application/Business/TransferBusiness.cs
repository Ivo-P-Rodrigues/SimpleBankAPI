using SimpleBank.AcctManage.Core.Application.Contracts.Business;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Core.Application.Contracts.Providers.Notification;
using SimpleBank.AcctManage.Core.Application.Models;
using SimpleBank.AcctManage.Core.Domain;
using Microsoft.Extensions.Configuration;
using System.Transactions;
using Serilog;
using System.Security.Cryptography.Xml;

namespace SimpleBank.AcctManage.Core.Application.Business
{
    public class TransferBusiness : ITransferBusiness
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransferNotificationProducer _notificationProducer;
        private readonly ILogger _logger;

        public TransferBusiness(
            IUnitOfWork unitOfWork,
            ITransferNotificationProducer notificationProducer,
            ILogger logger
        )
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _notificationProducer = notificationProducer ?? throw new ArgumentNullException(nameof(notificationProducer));
            _logger = logger;
        }


        //make transfer
        public async Task<(bool, bool, Transfer?)> MakeTransfer(Transfer transfer, Guid userId)
        {
            var accountFrom = await _unitOfWork.Accounts.GetAsync(transfer.FromAccountId);
            var accountTo = await _unitOfWork.Accounts.GetAsync(transfer.ToAccountId);

            if (!ValidateTransfer(transfer, accountFrom, accountTo)) { return (false, true, null); } //not valid 
            if (accountFrom!.UserId != userId) { return (true, false, null); }                       //unauthorized - user doesn't own accountFrom

            Transfer transferProcessed;
            try
            {
                transferProcessed = await ProcressTransferAsync(transfer, accountFrom, accountTo!);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Critical error on transfer processing.");
                return (true, true, null);
            }

            await SendMailNotification(transfer, userId, accountTo!.UserId);

            return (false, false, transferProcessed);
        }



        private bool ValidateTransfer(Transfer transfer, Account? accountFrom, Account? accountTo)
        {
            if (transfer.FromAccountId == transfer.ToAccountId) { return false; } //not valid - transfer to self 
            if (accountFrom == null || accountTo == null) { return false; }       //not valid - non existing account
            if (accountFrom.Balance < transfer.Amount) { return false; }          //not valid - non enough balance
            if (accountFrom.Currency != accountTo.Currency) { return false; }     //not valid - not same currency
            return true;
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
        private void CreateTransferMovements(Transfer transfer, out Movement movementFrom, out Movement movementTo)
        {
            movementFrom = new Movement()
            {
                AccountId = transfer.FromAccountId,
                Amount = -transfer.Amount,
                CreatedAt = DateTime.UtcNow
            };
            movementTo = new Movement()
            {
                AccountId = transfer.ToAccountId,
                Amount = transfer.Amount,
                CreatedAt = DateTime.UtcNow
            };
        }

        private async Task SendMailNotification(Transfer transfer, Guid fromUserId, Guid toUserId)
        {
            var userFrom = _unitOfWork.Users.Get(fromUserId);
            var userTo = _unitOfWork.Users.Get(toUserId);

            TransferMailNotification transferNotification = new TransferMailNotification(
                userFrom!.Username,
                userTo!.Username,
                transfer.ToAccountId.ToString(),
                userTo.Email,
                transfer.Amount.ToString());

            await _notificationProducer.RegisterTransferAsync(transferNotification);
        }

    }
}
