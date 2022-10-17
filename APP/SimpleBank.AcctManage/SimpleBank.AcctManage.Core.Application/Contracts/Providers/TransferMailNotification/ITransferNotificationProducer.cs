
using SimpleBank.AcctManage.Core.Application.Models;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Providers.Notification
{
    public interface ITransferNotificationProducer
    {
        Task<bool> RegisterTransferAsync(TransferMailNotification notification);




    }
}