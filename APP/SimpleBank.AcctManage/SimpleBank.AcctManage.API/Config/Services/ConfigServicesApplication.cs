using SimpleBank.AcctManage.Core.Application.Business;
using SimpleBank.AcctManage.Core.Application.Contracts.Business;

namespace SimpleBank.AcctManage.API.Config.Services
{
    public static class ConfigServicesApplication
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IUserBusiness, UserBusiness>();
            services.AddTransient<ISessionBusiness, SessionBusiness>();
            services.AddTransient<IAccountBusiness, AccountBusiness>();
            services.AddTransient<ITransferBusiness, TransferBusiness>();


            return services;
        }

    }
}
