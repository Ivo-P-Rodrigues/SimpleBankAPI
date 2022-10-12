namespace SimpleBank.AcctManage.Core.Application.Contracts.Providers.Notification
{
    public interface INotificationProvider<T> where T : class
    {
        void SendNotification(T notification);

        T DeserializeNotice(string messageValue);

    }
}
