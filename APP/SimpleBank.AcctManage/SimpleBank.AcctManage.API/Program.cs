using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using SimpleBank.AcctManage.API.Config.Hosts;
using SimpleBank.AcctManage.API.Config.Services;
using SimpleBank.AcctManage.API.Config.Swagger;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable = true);
builder.Services.AddEndpointsApiExplorer();

//Serilog
builder.Logging.ClearProviders();
builder.Host.RegisterSerilog();
builder.Services.AddSingleton(Log.Logger);

//API
builder.Services.ConfigureBusinessServicesV1();
builder.Services.ConfigureBusinessServicesV2();
builder.Services.ConfigureProvidersServices();
builder.Services.ConfigureBearerAuthService(builder.Configuration);
builder.Services.ConfigureApiVersioningService();
builder.Services.ConfigureSwaggerService();

//Infrastructure
builder.Services.ConfigurePersistenceServices(builder.Configuration); //Persistence
builder.Services.ConfigureTransferNotificationService(builder.Configuration, Log.Logger); //Host Providers (consumer)

var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            o.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                $"SimpleBankAPI - {description.GroupName.ToUpper()}");
        }
    });
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => app.MapControllers());

app.Run();