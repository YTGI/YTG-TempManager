using YTG.TempManager.Services;

namespace YTG.TempManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostApplicationBuilder? builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Worker>();

            builder.Services.AddWindowsService(options =>
            {
                options.ServiceName = "YTG Temp Manager Service";
            });

            ConfigurationBuilder _configuration = new();
            IConfiguration _configurationBuilder = _configuration
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            builder.Services.Configure<YTGAppSettings>(_configurationBuilder.GetSection("AppSettings"));
            builder.Services.AddSingleton<ITFService, TFService>();

            IHostBuilder _host = Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    if (OperatingSystem.IsWindows())
                    {
                        logging.AddEventLog();
                    }
                });

            var host = builder.Build();
            host.Run();
        }
    }
}