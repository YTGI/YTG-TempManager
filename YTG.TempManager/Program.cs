using YTG.TempManager.Services;

HostApplicationBuilder? builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<YTG.TempManager.Worker>();

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "YTG Temp Manager Service";
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
if (OperatingSystem.IsWindows())
{
    builder.Logging.AddEventLog();
}

builder.Services.Configure<YTG.TempManager.YTGAppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddSingleton<ITFService, TFService>();

using IHost? host = builder.Build();

try
{
    await host.RunAsync();
}
catch (Exception ex)
{
    //Stop error from being logged to event viewer when the windows service is stopped
    File.WriteAllText("C:\\Temp\\YTG.TempManager.Service-Program.log", ex.ToString());
    Environment.ExitCode = 0;
}
