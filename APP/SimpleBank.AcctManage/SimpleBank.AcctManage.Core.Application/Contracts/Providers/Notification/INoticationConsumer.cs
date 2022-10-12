using Confluent.Kafka;

namespace SimpleBank.AcctManage.Core.Application.Contracts.Providers.Notification
{
    public interface INoticationConsumer
    {
        IConsumer<Ignore, string> BuildConsumer();
    }
}