using Serilog;
using Serilog.Events;

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
             .Filter.ByExcluding((le) => le.Level == LogEventLevel.Information)
             .CreateLogger();

            host.UseSerilog();
        }

            
    }
}

//.Filter.ByIncludingOnly(isNotInfoLevel)
//private static bool isNotInfoLevel(LogEvent le)
//{
//    return le.Level != LogEventLevel.Information;
//}


//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();

