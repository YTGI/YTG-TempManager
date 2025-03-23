using YTG.TempManager.Services;

{
    HostApplicationBuilder? builder = Host.CreateApplicationBuilder(args);

    builder.Services.AddHostedService<YTG.TempManager.Worker>();

    builder.Services.AddWindowsService(options =>
    {
        options.ServiceName = "YTG Temp Manager Service";
    });

    ConfigurationBuilder _configuration = new();
    IConfiguration _configurationBuilder = _configuration
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();

    builder.Services.Configure<YTG.TempManager.YTGAppSettings>(_configurationBuilder.GetSection("AppSettings"));
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
    await host.RunAsync();

}
