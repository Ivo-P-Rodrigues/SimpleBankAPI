
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Core.Application.Contracts.Providers.Notification;
using SimpleBank.AcctManage.Core.Application.Models.Notification;
using SimpleBank.AcctManage.Infrastructure.Persistence.Repositories;
using SimpleBank.AcctManage.Infrastructure.Persistence.Repositories.Common;
using SimpleBank.AcctManage.Infrastructure.Providers.Notification.ConsumerService;
using SimpleBank.AcctManage.Infrastructure.Providers.NotificationManager.ConsumerService;
using SimpleBank.AcctManage.Infrastructure.Providers.NotificationManager.ProducerService;
using SimpleBank.AcctManage.Infrastructure.Providers.NotificationManager.Providers;
using Confluent.Kafka;

namespace SimpleBank.AcctManage.API.Config.Hosts
{
    public static class ConfigHostTransferNotification
    {
        public static void ConfigureTransferNotificationService(this IServiceCollection services, IConfiguration configuration, Serilog.ILogger logger)
        {

            services.AddTransient(typeof(INotificationProvider<>), typeof(NotificationEmailProvider<>));
            services.AddTransient(typeof(INotification), typeof(MailNotification));
            services.AddSingleton(typeof(INoticationConsumer<>), typeof(NoticationConsumer));
            services.AddSingleton(typeof(IConsumer<>), typeof(NoticationConsumer));
       
            services.AddTransient<INotificationProducer, NotificationProducer>();
            services.AddHostedService<NotificationConsumerThread>();
        }

    }
}



//sp.GetRequiredService<Serilog.ILogger>()

//var consumer = new NoticationConsumer(configuration).BuildConsumer();
//var emailProvider = new NotificationEmailProvider<MailNotification>(configuration, logger);

//services.AddHostedService(sp =>
//    new NotificationConsumerThread(
//        consumer,
//        emailProvider,
//        logger));