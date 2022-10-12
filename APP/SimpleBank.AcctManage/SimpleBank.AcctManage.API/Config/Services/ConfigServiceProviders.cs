using SimpleBank.AcctManage.API.Providers;
using SimpleBank.AcctManage.Core.Application.Contracts.Providers.Notification;
using SimpleBank.AcctManage.Infrastructure.Providers.Notification.ProducerService;

namespace SimpleBank.AcctManage.API.Config.Services
{
    public static class ConfigServiceProviders
    {
        public static IServiceCollection ConfigureProvidersServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthenthicationProvider, AuthenthicationProvider>();
            services.AddTransient<IEntityMapper, EntityMapper>();

            services.AddTransient<INotificationProducer, NotificationProducer>();
            return services;
        }


    }
}

