using SimpleBank.AcctManage.Core.Application.Models.Notification;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Providers.Notification
{
    public interface INotificationProvider<T> where T : INotification
    {
        void SendNotification(T notification);
        T DeserializeNotice(string messageValue);

    }
}
