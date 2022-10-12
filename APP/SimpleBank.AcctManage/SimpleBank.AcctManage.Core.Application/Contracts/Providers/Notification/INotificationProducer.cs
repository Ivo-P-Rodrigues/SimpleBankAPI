using SimpleBank.AcctManage.Core.Application.NotificationModels;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Providers.Notification
{
    public interface INotificationProducer
    {
        Task<bool> RegisterNotificationAsync(TransferMailNotice notification);




    }
}