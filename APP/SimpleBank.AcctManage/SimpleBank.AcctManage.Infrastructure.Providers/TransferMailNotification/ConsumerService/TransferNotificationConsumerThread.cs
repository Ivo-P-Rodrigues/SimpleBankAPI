using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Serilog;
using SimpleBank.AcctManage.Core.Application.Contracts.Providers.Notification;

namespace SimpleBank.AcctManage.Infrastructure.Providers.Notification.ConsumerService
{
    public class TransferNotificationConsumerThread<T> : BackgroundService where T : class 
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ITransferNotificationProvider _notificationProvider;
        private readonly ILogger _logger;


        public TransferNotificationConsumerThread(
            IConsumer<Ignore, string> consumer,
            ITransferNotificationProvider notificationProvider,
            ILogger logger)
        {
            _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
            _notificationProvider = notificationProvider ?? throw new ArgumentNullException(nameof(notificationProvider));
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();

            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);

                if (consumeResult != null)
                {
                    var notice = _notificationProvider.DeserializeNotice(consumeResult.Message.Value);
                    _notificationProvider.SendNotification(notice);

                    _logger.Information($"- KEY: {consumeResult.Message.Key}  | VALUE: {consumeResult.Message.Value} - ");
                }

                _consumer.Commit();
            }
        }

        public override void Dispose()
        {
            _consumer.Dispose();
            base.Dispose();
        }


    }
}
