
using Serilog;

namespace SimpleBankAPI.Core.BuildExtensions
{
    public static class HostExtensions
    {
        public static void RegisterSerilog(this IHostBuilder host)
        {
            Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Debug()
             .WriteTo.Console()
             .WriteTo.File("logs/SimpleBankAPI_Log.txt", rollingInterval: RollingInterval.Day)
             .CreateLogger();

            host.UseSerilog();
        }
    

            //builder.Logging.ClearProviders();
            //builder.Logging.AddConsole();

    }
}
