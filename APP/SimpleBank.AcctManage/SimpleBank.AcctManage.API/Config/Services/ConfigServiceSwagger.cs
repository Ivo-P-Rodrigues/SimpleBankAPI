using Microsoft.OpenApi.Models;
using System.Reflection;

namespace SimpleBank.AcctManage.API.Config.Services
{
    public static class ConfigServiceSwagger
    {
        public static void ConfigureSwaggerService(this IServiceCollection collection)
        {
            collection.AddSwaggerGen(options =>
            {
                //xml documentation
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                options.IncludeXmlComments(xmlCommentsFullPath);

                //add security to swagger
                options.AddSecurityDefinition("SimpleBankAPIBearerAuth", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Description = "Input a valid token to access this API."
                });

                //set token in request headers 
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

