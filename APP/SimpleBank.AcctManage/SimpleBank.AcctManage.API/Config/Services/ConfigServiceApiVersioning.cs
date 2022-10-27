using Microsoft.AspNetCore.Mvc;

namespace SimpleBank.AcctManage.API.Config.Services
{
    public static class ConfigServiceApiVersioning
    {
        public static void ConfigureApiVersioningService(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true; //allow versioning
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        }

    }
}

