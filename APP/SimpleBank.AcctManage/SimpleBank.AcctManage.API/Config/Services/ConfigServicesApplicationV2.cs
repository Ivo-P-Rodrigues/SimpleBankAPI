using SimpleBank.AcctManage.Core.Application.Contracts.Business.v2;
using SimpleBank.AcctManage.Core.Application.Business.v2;

namespace SimpleBank.AcctManage.API.Config.Services
{
    public static class ConfigServicesApplicationV2
    {
        public static IServiceCollection ConfigureBusinessServicesV2(this IServiceCollection services)
        {
            services.AddTransient<IUserBusiness,UserBusiness>();
            services.AddTransient<IUserTokenBusiness, UserTokenBusiness>();
            services.AddTransient<IAccountBusiness, AccountBusiness>();
            services.AddTransient<ITransferBusiness, TransferBusiness>();
            services.AddTransient<IMovementBusiness, MovementBusiness>();
            services.AddTransient<IAccountDocBusiness, AccountDocBusiness>();

            return services;
        }

    }
}
