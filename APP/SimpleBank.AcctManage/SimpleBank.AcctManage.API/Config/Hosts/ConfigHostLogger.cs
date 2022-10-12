using Serilog;

namespace SimpleBank.AcctManage.API.Config.Hosts
{
    public static class ConfigHostLogger
    {
        public static void RegisterSerilog(this IHostBuilder host)
        {
            Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Debug()
             .WriteTo.Console()
             .WriteTo.File("_Logs/SimpleBankAPI_Log.txt", rollingInterval: RollingInterval.Day)
             .CreateLogger();

            host.UseSerilog();
        }

            
    }
}

//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();

