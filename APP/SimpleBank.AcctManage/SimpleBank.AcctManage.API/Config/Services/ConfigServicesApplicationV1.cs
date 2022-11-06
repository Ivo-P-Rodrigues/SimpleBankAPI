using SimpleBank.AcctManage.Core.Application.Contracts.Business.v1;
using SimpleBank.AcctManage.Core.Application.Business.v1;

namespace SimpleBank.AcctManage.API.Config.Services
{
    public static class ConfigServicesApplicationV1
    {
        public static IServiceCollection ConfigureBusinessServicesV1(this IServiceCollection services)
        {
            services.AddTransient<IUserBusiness, UserBusiness>();
            services.AddTransient<IUserTokenBusiness, UserTokenBusiness>();
            services.AddTransient<IAccountBusiness, AccountBusiness>();
            services.AddTransient<ITransferBusiness, TransferBusiness>();

            return services;
        }

    }
}
