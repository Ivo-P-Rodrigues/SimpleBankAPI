using SimpleBank.AcctManage.Core.Application.Models.Notification;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Providers.Notification
{
    public interface INotificationProducer
    {
        Task<bool> RegisterNotificationAsync(INotification notification);




    }
}