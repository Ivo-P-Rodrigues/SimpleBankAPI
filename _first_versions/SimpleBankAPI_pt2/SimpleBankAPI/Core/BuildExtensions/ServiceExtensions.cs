using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SimpleBankAPI.Business;
using SimpleBankAPI.Repository;
using System.Reflection;
using System.Text;
using SimpleBankAPI.WebAPI;

namespace SimpleBankAPI.Core.BuildExtensions
{
    public static class ServiceExtensions
    {
        public static void RegisterRepos(this IServiceCollection collection)
        {
            collection.AddTransient<IUserRepository, UserRepository>();
            collection.AddTransient<ITransferRepository, TransferRepository>();
            collection.AddTransient<IMovementRepository, MovementRepository>();
            collection.AddTransient<IAccountRepository, AccountRepository>();
            collection.AddTransient<ISessionRepository, SessionRepository>();

            collection.AddTransient<IUnitOfWork, UnitOfWork>();
        }
        public static void RegisterBusinesses(this IServiceCollection collection)
        {
            collection.AddTransient<ITransferBusiness, TransferBusiness>();
            collection.AddTransient<IUserBusiness, UserBusiness>();
            collection.AddTransient<IAccountBusiness, AccountBusiness>();
            collection.AddTransient<ISessionBusiness, SessionBusiness>();
        }
        public static void RegisterProviders(this IServiceCollection collection)
        {
            collection.AddTransient<IAuthenthicationProvider, AuthenthicationProvider>();
            collection.AddTransient<IEntityMapper, EntityMapper>();
        }



        public static void AddMyAuthenthication(this IServiceCollection collection, IConfiguration configuration)
        {
            collection.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Authentication:Issuer"],
                        ValidAudience = configuration["Authentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(configuration["Authentication:SecretKey"]))
                    };
                });
        }



        public static void AddMySwagger(this IServiceCollection collection)
        {

            collection.AddSwaggerGen(options =>
            {
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; //add xml documentation to swagger
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                options.IncludeXmlComments(xmlCommentsFullPath); //in project properties -> output -> xml documentation -> write the name of the proj: SimpleBankAPI.xml

                options.AddSecurityDefinition("SimpleBankAPIBearerAuth", new OpenApiSecurityScheme() //add security to swagger
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Description = "Input a valid token to access this API."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement //set token in request headers   //Dict with OpenApiSecurityScheme as key
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "SimpleBankAPIBearerAuth" }
                    }, new List<string>() }
                 });
            });

        }



    }
}
