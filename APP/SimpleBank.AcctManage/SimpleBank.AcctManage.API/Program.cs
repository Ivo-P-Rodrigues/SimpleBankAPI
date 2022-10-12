using Serilog;
using SimpleBank.AcctManage.API.Config.Hosts;
using SimpleBank.AcctManage.API.Config.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable = true);
builder.Services.AddEndpointsApiExplorer();

//Serilog
builder.Logging.ClearProviders();
builder.Host.RegisterSerilog();
builder.Services.AddSingleton(Log.Logger);

//Core - Application
builder.Services.ConfigureApplicationServices();

//API
builder.Services.ConfigureProvidersServices();
builder.Services.ConfigureBearerAuthService(builder.Configuration);
builder.Services.ConfigureSwaggerService();

//Infrastructure - Persistence
builder.Services.ConfigurePersistenceServices(builder.Configuration);

//Infrastructure - Host Providers (consumer)
builder.Services.ConfigureTransferNotificationService(builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => app.MapControllers());

app.Run();