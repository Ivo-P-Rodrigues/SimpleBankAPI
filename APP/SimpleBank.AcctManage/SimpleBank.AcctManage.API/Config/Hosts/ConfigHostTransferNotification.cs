using SimpleBank.AcctManage.Core.Application.NotificationModels;
using SimpleBank.AcctManage.Infrastructure.Providers.Notification.ConsumerService;
using SimpleBank.AcctManage.Infrastructure.Providers.Notification.Providers;

namespace SimpleBank.AcctManage.API.Config.Hosts
{
    public static class ConfigHostTransferNotification
    {
        public static void ConfigureTransferNotificationService(this IServiceCollection services, IConfiguration configuration, Serilog.ILogger logger)
        {
            var consumer = new TransferNoticationConsumer(configuration).BuildConsumer();
            var emailProvider = new TransferNotificationEmailProvider(configuration, logger);

            services.AddHostedService(sp =>
                new TransferNotificationConsumerThread<TransferMailNotification>(
                    consumer,
                    emailProvider,
                    logger));
            
        }

    }
}


//sp.GetRequiredService<Serilog.ILogger>()