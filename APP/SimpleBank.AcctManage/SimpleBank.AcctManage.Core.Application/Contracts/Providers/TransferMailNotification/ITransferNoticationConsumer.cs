namespace SimpleBank.AcctManage.Infrastructure.Providers.Notification.ConsumerService
{
    public interface ITransferNoticationConsumer<T> where T : class
    {
        T BuildConsumer();
    }
}