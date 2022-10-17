using Microsoft.EntityFrameworkCore;
using SimpleBank.AcctManage.Core.Application.Contracts.Persistence;
using SimpleBank.AcctManage.Infrastructure.Persistence;
using SimpleBank.AcctManage.Infrastructure.Persistence.Repositories;
using SimpleBank.AcctManage.Infrastructure.Persistence.Repositories.Common;

namespace SimpleBank.AcctManage.API.Config.Services
{
    public static class ConfigServicePersistence
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SimpleBankDbContext>(options =>
               options.UseNpgsql(configuration.GetConnectionString("DbConnection")));

            services.AddScoped(typeof(ICommonRepository<>), typeof(CommonRepository<>));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITransferRepository, TransferRepository>();
            services.AddScoped<IMovementRepository, MovementRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

    }
}
