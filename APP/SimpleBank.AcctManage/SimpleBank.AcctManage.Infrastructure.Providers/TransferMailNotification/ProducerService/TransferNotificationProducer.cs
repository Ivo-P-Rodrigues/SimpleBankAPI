using Confluent.Kafka;
using Serilog;
using Microsoft.Extensions.Configuration;
using SimpleBank.AcctManage.Core.Application.Contracts.Providers.Notification;
using System.Text.Json;
using SimpleBank.AcctManage.Core.Application.Models;

namespace SimpleBank.AcctManage.Infrastructure.Providers.Notification.ProducerService
{
    public class TransferNotificationProducer : ITransferNotificationProducer
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public TransferNotificationProducer(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger;
        }


        public async Task<bool> RegisterTransferAsync(TransferMailNotification notification)
        {
            string notificationStringed = JsonSerializer.Serialize(notification);

            var producerConfig = _configuration.GetSection("ProducerConfig").Get<ProducerConfig>();

            try
            {
                using (var producer = new ProducerBuilder<Null, string>(producerConfig).Build())
                {
                    var result = await producer.ProduceAsync(_configuration["KafkaTopic"], new Message<Null, string>
                    {
                        Value = notificationStringed
                    });

                    return await Task.FromResult(true);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error on producing notification", ex);
            }

            return await Task.FromResult(false);
        }


    }
}
