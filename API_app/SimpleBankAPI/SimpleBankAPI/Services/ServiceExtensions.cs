using SimpleBankAPI.Business;
using SimpleBankAPI.Repository;

namespace SimpleBankAPI.Services
{
    public static class ServiceExtensions
    {
        public static void RegisterRepos(this IServiceCollection collection)
        {
            collection.AddScoped<IUserRepository, UserRepository>();
            collection.AddScoped<ITransferRepository, TransferRepository>();
            collection.AddScoped<IMovementRepository, MovementRepository>();
            collection.AddScoped<IAccountRepository, AccountRepository>();
            collection.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        public static void RegisterBusinesses(this IServiceCollection collection)
        {
            collection.AddScoped<ITransferBusiness, TransferBusiness>();
            collection.AddScoped<IUserBusiness, UserBusiness>();
            collection.AddScoped<IAccountBusiness, AccountBusiness>();
        }
        public static void Miscellaneous(this IServiceCollection collection)
        {
            collection.AddTransient<IHardEntityMapper, HardEntityMapper>();
            collection.AddTransient<ITokenGenerator, TokenGenerator>();
        }



    }
}
