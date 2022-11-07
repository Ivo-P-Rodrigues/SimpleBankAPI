using Microsoft.IdentityModel.Tokens;
using SimpleBank.AcctManage.Core.Application.Contracts.Providers;
using SimpleBank.AcctManage.Infrastructure.Auth;

namespace SimpleBank.AcctManage.API.Config.Services
{
    public static class ConfigServiceBearerAuth
    {
        public static void ConfigureBearerAuthService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        RequireSignedTokens = true,
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["Authentication:Issuer"],
                        ValidAudience = configuration["Authentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            System.Text.Encoding.ASCII.GetBytes(configuration["Authentication:SecretKey"]))
                    };
                });

            services.AddTransient<IAuthenthicationProvider, AuthenthicationProvider>();
        }

    }
}

