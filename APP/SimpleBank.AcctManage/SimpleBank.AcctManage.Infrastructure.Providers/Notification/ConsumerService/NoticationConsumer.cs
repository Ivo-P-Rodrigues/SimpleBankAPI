using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using SimpleBank.AcctManage.Core.Application.Contracts.Providers.Notification;

namespace SimpleBank.AcctManage.Infrastructure.Providers.Notification.ConsumerService
{
    public class NoticationConsumer : INoticationConsumer
    {
        private readonly IConfiguration _configuration;
        public NoticationConsumer(IConfiguration configuration)
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
