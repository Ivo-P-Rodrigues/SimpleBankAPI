using SimpleBank.AcctManage.Core.Application.Contracts.Providers.Notification;
using SimpleBank.AcctManage.Infrastructure.Providers.Notification.ProducerService;

namespace SimpleBank.AcctManage.API.Config.Services
{
    public static class ConfigServiceProviders
    {
        public static IServiceCollection ConfigureProvidersServices(this IServiceCollection services)
        {
            services.AddTransient<Mapping.v2.IEntityMapper, Mapping.v2.EntityMapper>();
            services.AddTransient<Mapping.v1.IEntityMapper, Mapping.v1.EntityMapper>();

            services.AddTransient<ITransferNotificationProducer, TransferNotificationProducer>();
            return services;
        }


    }
}

