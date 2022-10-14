namespace SimpleBank.AcctManage.Infrastructure.Providers.Notification.ConsumerService
{
    public interface INoticationConsumer<T> where T : class
    {
        T BuildConsumer();
    }
}