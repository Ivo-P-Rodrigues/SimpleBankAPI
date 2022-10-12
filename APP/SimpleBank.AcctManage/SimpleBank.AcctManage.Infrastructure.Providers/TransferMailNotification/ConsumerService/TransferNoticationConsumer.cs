using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace SimpleBank.AcctManage.Infrastructure.Providers.Notification.ConsumerService
{
    public class TransferNoticationConsumer : ITransferNoticationConsumer<IConsumer<Ignore, string>>
    {
        private readonly IConfiguration _configuration;
        public TransferNoticationConsumer(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IConsumer<Ignore, string> BuildConsumer()
        {
            var consumerConfig = _configuration.GetSection("ConsumerConfig").Get<ConsumerConfig>();
            var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            consumer.Subscribe(_configuration["KafkaTopic"]);
            return consumer;
        }



    }
}
