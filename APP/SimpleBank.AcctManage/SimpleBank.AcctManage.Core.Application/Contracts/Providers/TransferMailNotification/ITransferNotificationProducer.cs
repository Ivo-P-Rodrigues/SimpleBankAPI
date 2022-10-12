
using SimpleBank.AcctManage.Core.Application.NotificationModels;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Providers.Notification
{
    public interface ITransferNotificationProducer
    {
        Task<bool> RegisterTransferAsync(TransferMailNotification notification);




    }
}